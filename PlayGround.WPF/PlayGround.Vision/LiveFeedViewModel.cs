using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows.Media.Imaging;

namespace PlayGround.Vision;

public class LiveFeedViewModel : ReactiveObject, IDisposable
{
    private readonly ObservableAsPropertyHelper<BitmapImage> _backingOriginalImage;
    private readonly CompositeDisposable _compositeDisposable = new();
    private readonly IVideoService _videoService;

    public LiveFeedViewModel(IVideoService videoService)
    {
        _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
        _backingOriginalImage = _videoService.OriginalImage.ToProperty(this, nameof(OriginalImage));
        if (!_videoService.IsPlaying)
            _videoService.Play(true);
    }

    public BitmapImage OriginalImage => _backingOriginalImage.Value;

    public void Dispose()
    {
        if (!_videoService.IsDisposed)
            _videoService.Dispose();
        _compositeDisposable.Dispose();
    }
}