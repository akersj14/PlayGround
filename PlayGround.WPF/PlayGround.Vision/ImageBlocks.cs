using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using OpenCvSharp;

namespace PlayGround.Vision;

public class LiveFeedBlock : BasicDisposableBlock
{
    private readonly Subject<Mat> _backingLiveFeed = new();
    
    public LiveFeedBlock()
    {
        var liveFeedService = VisionModule.Resolve<ILiveFeedService>();
        Setup();
        Title = "Live Feed Block";
        OutputFieldNames = new List<string> {"Image"};
        OutputTypes = new List<Type> {typeof(IObservable<Mat>)};
        Outputs = new List<object?> {null};
        Outputs[0] = _backingLiveFeed.AsObservable();
        
        liveFeedService
            .MatImage
            .Subscribe(mat =>
            {
                _backingCompleted.OnNext(true);
                _backingLiveFeed.OnNext(mat);
            })
            .DisposeWith(CompositeDisposable);
    }

    protected override bool Connect()
    {
        return true;
    }
}

public class ProcessedBlock : BasicDisposableBlock
{
    private readonly IProcessedImageService _processedImageService;
    public ProcessedBlock()
    {
        _processedImageService = VisionModule.Resolve<IProcessedImageService>();
        
        Setup();
        Title = "Processed Block";
        InputFieldNames = new List<string> {"Image"};
        InputTypes = new List<Type> {typeof(IObservable<Mat>)};
    }

    protected override bool Connect()
    {
        var observable = (IObservable<Mat>)Inputs[0]!;
        observable
            .Subscribe(mat =>
            {
                _backingCompleted.OnNext(true);
                _processedImageService.SetNextProcessedImage(mat);
            })
            .DisposeWith(CompositeDisposable);
        return true;
    }
}