using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows.Media.Imaging;

namespace PlayGround.Vision;

public class LiveFeedViewModel : ReactiveObject, IDisposable
{
  private readonly ObservableAsPropertyHelper<BitmapImage> _backingImage;
  private readonly CompositeDisposable _compositeDisposable = new();
  private readonly IVideoService _videoService;

  public LiveFeedViewModel(IVideoService videoService)
  {
    _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
    _backingImage = _videoService.Image.ToProperty(this, nameof(Image));

    _videoService.Play();
  }

  public BitmapImage Image => _backingImage.Value;

  public void Dispose()
  {
    _videoService.Dispose();
    _compositeDisposable.Dispose();
  }
}