using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using PlayGround.Basics;

namespace PlayGround.Vision;

public interface ILiveFeedService
{
    IObservable<BitmapImage> BitmapImage { get; }
    IObservable<Mat> MatImage { get; }
}

public class LiveFeedService : BasicDisposable, ILiveFeedService
{
    private readonly Subject<BitmapImage> _backingBitmapImage = new();
    private readonly Subject<Mat> _backingMatImage = new();
    
    public LiveFeedService(IVideoService videoService)
    {
        if (!videoService.IsPlaying)
            videoService.Play();
        
        BitmapImage = _backingBitmapImage.AsObservable();
        MatImage = _backingMatImage.AsObservable();
        
        videoService
            .OriginalImage
            .Subscribe(mat =>
            {
                _backingMatImage.OnNext(mat);
                _backingBitmapImage.OnNext(Converters.MatToBitmapImage(mat));
            })
            .DisposeWith(CompositeDisposable);
    }
    
    public IObservable<BitmapImage> BitmapImage { get; }
    public IObservable<Mat> MatImage { get; }
}