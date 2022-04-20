using System.Drawing;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PlayGround.Vision;

public interface IVideoService : IDisposable
{
  Task Play();
  IObservable<BitmapImage> Image { get; }
}

public class VideoService : IVideoService
{
  private readonly VideoCapture _videoCapture;
  private readonly Subject<BitmapImage> _backingImage = new();
  private readonly int _sleepTime;
  private readonly IImageProcessor _imageProcessor;

  public VideoService(IImageProcessor imageProcessor)
  {
    _imageProcessor = imageProcessor ?? throw new ArgumentNullException(nameof(imageProcessor));
    Image = _backingImage.AsObservable();
    _videoCapture = VideoCapture.FromCamera(0);
    _sleepTime = (int)Math.Round(1000 / _videoCapture.Fps);
  }
  public async Task Play()
  {
    using var frame = new Mat();
    while (_videoCapture.IsOpened())
    {
      if (_videoCapture.Read(frame))
      {
        var result = _imageProcessor.Process(frame);
        _backingImage.OnNext(BitmapToImageSource(result.ToBitmap()));
        result.Dispose();
      }
      await Task.Delay(_sleepTime);
    }
  }

  public IObservable<BitmapImage> Image { get; }

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
    _videoCapture.Dispose();
  }
}