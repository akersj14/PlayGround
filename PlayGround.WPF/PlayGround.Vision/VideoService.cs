using OpenCvSharp;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace PlayGround.Vision;

public interface IVideoService : IDisposable
{
  Task Play();
  IObservable<Mat> OriginalImage { get; }
  bool IsDisposed { get; }
  bool IsPlaying { get; }
}

public class VideoService : IVideoService
{
  private readonly VideoCapture _videoCapture;
  private readonly Subject<Mat> _backingOriginalImage = new();
  private readonly int _sleepTime;

  public VideoService(IImageProcessor imageProcessor)
  {
    OriginalImage = _backingOriginalImage.AsObservable();
    _videoCapture = VideoCapture.FromCamera(0);
    _sleepTime = (int)Math.Round(1000 / _videoCapture.Fps);
    IsDisposed = false;
    Play();
  }

  public async Task Play()
  {
    IsPlaying = true;
    using var frame = new Mat();
    while (_videoCapture.IsOpened())
    {
      if (_videoCapture.Read(frame))
      {
        _backingOriginalImage.OnNext(frame);
      }
      await Task.Delay(_sleepTime);
    }
  }

  public IObservable<Mat> OriginalImage { get; }
  public bool IsDisposed { get; private set; }
  public bool IsPlaying { get; private set; }

  public void Dispose()
  {
    IsPlaying = false;
    IsDisposed = true;
    if (!_videoCapture.IsDisposed)
      _videoCapture.Dispose();
  }
}