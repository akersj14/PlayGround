using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using OpenCvSharp;

namespace PlayGround.Vision;

public class KeepSame : BasicDisposableBlock
{
    private readonly Subject<Mat> _backingOutput = new();
    public KeepSame()
    {
        Setup();
        Title = "Keep Same";
        InputFieldNames = new List<string> {"Image"};
        OutputFieldNames = new List<string> {"Image"};
        InputTypes = new List<Type> {typeof(IObservable<Mat>)};
        OutputTypes = new List<Type> {typeof(IObservable<Mat>)};
        Outputs = new List<object?> {null};
        Outputs[0] = _backingOutput.AsObservable();
        _backingErrored.OnNext(false);
        _backingErrorMessage.OnNext("");
    }

    protected override bool Connect()
    {
        if (Inputs.Count < InputTypes.Count || Inputs.Any(item => item == null)) 
            return false;
        var inputObservable = (IObservable<Mat>)Inputs[0]!;
        
        inputObservable
            .Subscribe(mat =>
            {
                _backingOutput.OnNext(mat.Clone());
                _backingCompleted.OnNext(true);
            })
            .DisposeWith(CompositeDisposable);
        return true;
    }
}

public class Canny : BasicDisposableBlock
{
    private readonly Subject<Mat> _backingOutput = new();
    
    public Canny()
    {
        Setup();
        Title = "Canny";
        InputFieldNames = new List<string> {"Image", "Threshold 1", "Threshold 2"};
        OutputFieldNames = new List<string> {"Image"};
        InputTypes = new List<Type> {typeof(IObservable<Mat>), typeof(IObservable<double>), typeof(IObservable<double>)};
        OutputTypes = new List<Type> {typeof(IObservable<Mat>)};
        Outputs = new List<object?> {null};
        Outputs[0] = _backingOutput.AsObservable();
        _backingErrored.OnNext(false);
        _backingErrorMessage.OnNext("");
    }

    protected override bool Connect()
    {
        if (Inputs.Count < InputTypes.Count || Inputs.Any(item => item == null)) 
            return false;
        var inputObservable = (IObservable<Mat>)Inputs[0]!;
        var inputThreshold1Observable = (IObservable<double>) Inputs[1]!;
        var inputThreshold2Observable = (IObservable<double>) Inputs[2]!;
        
        inputThreshold1Observable
            .Subscribe(value => Threshold1 = value)
            .DisposeWith(CompositeDisposable);
        
        inputThreshold2Observable
            .Subscribe(value => Threshold2 = value)
            .DisposeWith(CompositeDisposable);
        
        inputObservable
            .Subscribe(mat =>
            {
                _backingOutput.OnNext(mat.Canny(Threshold1, Threshold2));
                _backingCompleted.OnNext(true);
            })
            .DisposeWith(CompositeDisposable);
        return true;
    }

    private double Threshold1 { get; set; }
    private double Threshold2 { get; set; }
}