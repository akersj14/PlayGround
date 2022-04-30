using ReactiveUI;
using System.Windows.Media.Imaging;

namespace PlayGround.Vision;

public class LiveFeedViewModel : ReactiveObject
{
    private readonly ObservableAsPropertyHelper<BitmapImage> _backingOriginalImage;
    
    public LiveFeedViewModel(ILiveFeedService liveFeedService)
    {
        if (liveFeedService == null) throw new ArgumentNullException(nameof(liveFeedService));
        _backingOriginalImage = liveFeedService.BitmapImage.ToProperty(this, nameof(OriginalImage));
    }

    public BitmapImage OriginalImage => _backingOriginalImage.Value;

}