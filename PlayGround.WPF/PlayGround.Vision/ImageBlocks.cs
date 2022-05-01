using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using OpenCvSharp;

namespace PlayGround.Vision;

public class LiveFeedBlock : BasicDisposableBlock
{
    private readonly Subject<Mat> _backingLiveFeed = new();
    
    public LiveFeedBlock(ILiveFeedService liveFeedService)
    {
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
    private readonly Subject<Mat> _backingProcessedFeed = new();
    
    public ProcessedBlock()
    {
        Setup();
        Title = "Processed Block";
        InputFieldNames = new List<string> {"Image"};
        InputTypes = new List<Type> {typeof(IObservable<Mat>)};
        OutputFieldNames = new List<string> {"Processed Feed"};
        OutputTypes = new List<Type> {typeof(IObservable<Mat>)};
        Outputs = new List<object?> {null};
        Outputs[0] = _backingProcessedFeed.AsObservable();
    }

    protected override bool Connect()
    {
        var observable = (IObservable<Mat>)Inputs[0]!;
        observable
            .Subscribe(mat =>
            {
                _backingCompleted.OnNext(true);
                _backingProcessedFeed.OnNext(mat);
            })
            .DisposeWith(CompositeDisposable);
        return true;
    }
}