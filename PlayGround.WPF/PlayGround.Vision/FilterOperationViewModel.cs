using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI;

namespace PlayGround.Vision;

public class FilterOperationViewModel : ReactiveObject, IDisposable
{
    private bool _backingErrored;
    private CompositeDisposable _compositeDisposable = new();
    public FilterOperationViewModel(IOperation operation, IOperationsService operationsService)
    {
        if (operationsService == null) throw new ArgumentNullException(nameof(operationsService));
        Id = operation.Id;
        Remove = ReactiveCommand.Create(() =>
        {
            operationsService.RemoveWithId(Id);
        });
        Name = operation.GetType().Name;
        operation
            .WhenAnyValue(item => item.Errored)
            .Subscribe(errored => Errored = errored)
            .DisposeWith(_compositeDisposable);
    }
    public int Id { get; }
    public ReactiveCommand<Unit, Unit> Remove;
    public string Name { get; }
    public bool Errored
    {
        get => _backingErrored;
        set => this.RaiseAndSetIfChanged(ref _backingErrored, value);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}