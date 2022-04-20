using System.Reactive.Disposables;
using System.Windows.Media.Imaging;
using ReactiveUI;

namespace PlayGround.Vision;

public class ProcessedImageFeedViewModel : ReactiveObject, IDisposable
{
    private readonly ObservableAsPropertyHelper<BitmapImage> _backingProcessedImage;
    private readonly CompositeDisposable _compositeDisposable = new();
    private readonly IVideoService _videoService;

    public ProcessedImageFeedViewModel(IVideoService videoService)
    {
        _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
        _backingProcessedImage = _videoService.ProcessedImage.ToProperty(this, nameof(ProcessedImage));
        if (!_videoService.IsPlaying)
            _videoService.Play(true);
    }

    public BitmapImage ProcessedImage => _backingProcessedImage.Value;

    public void Dispose()
    {
        if (!_videoService.IsDisposed)
            _videoService.Dispose();
        _compositeDisposable.Dispose();
    }
}