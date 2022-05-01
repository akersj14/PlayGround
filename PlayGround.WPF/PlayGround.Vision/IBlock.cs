using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace PlayGround.Vision;

public interface IBlock
{
    int Id { get; set; }
    string Title { get; }
    List<string> InputFieldNames { get; }
    List<string> OutputFieldNames { get; }
    List<Type> InputTypes { get; }
    List<Type> OutputTypes { get; }
    List<object?> Outputs { get; }
    List<object?> Inputs { get; set; }
    List<IBlock> Dependencies { get; set; }
    IObservable<bool> Errored { get; }
    IObservable<string> ErrorMessage { get; }
    IObservable<bool> Completed { get; }
    bool AddDependency(string fieldName, IBlock dependency, Func<IBlock, object> getProperty);
}

public abstract class BasicBlock : IBlock
{
    protected Subject<bool> _backingErrored = new();
    protected Subject<string> _backingErrorMessage = new();
    protected Subject<bool> _backingCompleted = new();

    public int Id { get; set; }
    public string Title { get; protected set; } = "Basic Block";
    public List<string> InputFieldNames { get; protected set; } = new();
    public List<string> OutputFieldNames { get; protected set; } = new();
    public List<Type> InputTypes { get; protected set; } = new();
    public List<Type> OutputTypes { get; protected set; } = new();
    public List<object?> Outputs { get; protected set; }= new();
    public List<object?> Inputs { get; set; } = new();
    public List<IBlock> Dependencies { get; set; } = new();
    public IObservable<bool> Errored { get; protected set;}
    public IObservable<string> ErrorMessage { get; protected set;}
    public IObservable<bool> Completed { get; protected set; }

    protected void Setup()
    {
        Errored = _backingErrored.AsObservable();
        ErrorMessage = _backingErrorMessage.AsObservable();
        Completed = _backingCompleted.AsObservable();
    }
    
    public bool AddDependency(string fieldName, IBlock dependency, Func<IBlock, object> getProperty)
    {
        if (InputFieldNames.Count == 0 || !InputFieldNames.Contains(fieldName)) return false;
        var index = InputFieldNames.IndexOf(fieldName);
        if (InputTypes.Count <= index) return false;
        
        var expectedInputType = InputTypes[index];
        var property = getProperty(dependency);
        if (property.GetType() != expectedInputType) return false;
        
        if (Inputs.Count == 0)
            Inputs = Enumerable.Repeat<object?>(null, InputFieldNames.Count).ToList();
        if (Outputs.Count == 0)
            Outputs = Enumerable.Repeat<object?>(null, OutputFieldNames.Count).ToList();
        
        Inputs[index] = property;
        Dependencies.Add(dependency);
        return Connect();
    }

    protected abstract bool Connect();
}

public abstract class BasicDisposableBlock : BasicBlock, IDisposable
{
    protected readonly CompositeDisposable CompositeDisposable = new();
    public void Dispose() => CompositeDisposable.Dispose();
}