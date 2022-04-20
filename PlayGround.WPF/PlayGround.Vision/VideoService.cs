using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Media.Imaging;

namespace PlayGround.Vision;

public interface IVideoService : IDisposable
{
  Task Play(bool withProcessing);
  IObservable<BitmapImage> OriginalImage { get; }
  IObservable<BitmapImage> ProcessedImage { get; }
  bool IsDisposed { get; }
  bool IsPlaying { get; }
}

public class VideoService : IVideoService
{
  private readonly VideoCapture _videoCapture;
  private readonly Subject<BitmapImage> _backingOriginalImage = new();
  private readonly Subject<BitmapImage> _backingProcessedImage = new();
  private readonly int _sleepTime;
  private readonly IImageProcessor _imageProcessor;

  public VideoService(IImageProcessor imageProcessor)
  {
    _imageProcessor = imageProcessor ?? throw new ArgumentNullException(nameof(imageProcessor));
    OriginalImage = _backingOriginalImage.AsObservable();
    ProcessedImage = _backingProcessedImage.AsObservable();
    _videoCapture = VideoCapture.FromCamera(0);
    _sleepTime = (int)Math.Round(1000 / _videoCapture.Fps);
    IsDisposed = false;
  }

  public async Task Play(bool withProcessing)
  {
    IsPlaying = true;
    using var frame = new Mat();
    while (_videoCapture.IsOpened())
    {
      if (_videoCapture.Read(frame))
      {
        _backingOriginalImage.OnNext(BitmapToImageSource(frame.ToBitmap()));
        if (withProcessing)
        {
          var result = _imageProcessor.Process(frame);
          _backingProcessedImage.OnNext(BitmapToImageSource(result.ToBitmap()));
          result.Dispose();
        }
      }
      await Task.Delay(_sleepTime);
    }
  }

  public IObservable<BitmapImage> OriginalImage { get; }
  public IObservable<BitmapImage> ProcessedImage { get; }
  public bool IsDisposed { get; private set; }
  public bool IsPlaying { get; private set; }

  private static BitmapImage BitmapToImageSource(Image bitmap)
  {
    using var memory = new MemoryStream();
    bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
    memory.Position = 0;

    var bitmapImage = new BitmapImage();
    bitmapImage.BeginInit();
    bitmapImage.StreamSource = memory;
    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
    bitmapImage.EndInit();
    bitmapImage.Freeze();

    return bitmapImage;
  }
  public void Dispose()
  {
    IsPlaying = false;
    IsDisposed = true;
    _videoCapture.Dispose();
  }
}