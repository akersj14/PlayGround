using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;
using ReactiveUI;

namespace PlayGround.Vision;

public class ProcessedImageFeedViewModel : ReactiveObject, IDisposable
{
    private readonly ObservableAsPropertyHelper<BitmapImage> _backingProcessedImage;
    private readonly CompositeDisposable _compositeDisposable = new();

    public ProcessedImageFeedViewModel(IProcessedImageService processedImageService)
    {
        if (processedImageService == null) 
            throw new ArgumentNullException(nameof(processedImageService));
        _backingProcessedImage = processedImageService
            .ProcessedImage
            .Select(Converters.MatToBitmapImage)
            .ToProperty(this, nameof(ProcessedImage));
    }

    public BitmapImage ProcessedImage => _backingProcessedImage.Value;

    public void Dispose() => _compositeDisposable.Dispose();
}