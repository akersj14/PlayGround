using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using OpenCvSharp;

namespace PlayGround.Vision;

public class IntegerBlock : BasicDisposableBlock
{
    private readonly Subject<int> _backingIntOutput = new();

    public IntegerBlock()
    {
        Setup();
        Title = "int";
        OutputTypes = new List<Type> {typeof(IObservable<int>)};
        OutputFieldNames = new List<string> {"value"};
        Outputs = new List<object?> {null};
        Outputs[0] = _backingIntOutput.AsObservable();
    }

    protected override bool Connect() => true;

    public void SetValue(int number)
    {
        _backingCompleted.OnNext(true);
        _backingIntOutput.OnNext(number);
    }
}

public class SizeBlock : BasicDisposableBlock
{
    private readonly Subject<Size> _backingSizeOutput = new();

    public SizeBlock()
    {
        Setup();
        Title = "Size";

        InputFieldNames = new List<string> {"Height", "Width"};
        InputTypes = new List<Type> {typeof(IObservable<int>), typeof(IObservable<int>)};
        
        OutputTypes = new List<Type> {typeof(IObservable<Size>)};
        OutputFieldNames = new List<string> {"value"};
        Outputs = new List<object?> {null};
        Outputs[0] = _backingSizeOutput.AsObservable();
    }

    protected override bool Connect()
    {
        if (Inputs.Count != 2 || Inputs.Any(item => item == null)) 
            return true;
        
        var height = (IObservable<int>) Inputs[0]!;
        var width = (IObservable<int>) Inputs[1]!;
        
        height
            .CombineLatest(width)
            .Subscribe(tuple => _backingSizeOutput.OnNext(new Size(tuple.Second, tuple.First)))
            .DisposeWith(CompositeDisposable);
        
        return true;
    }

}