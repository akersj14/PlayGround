using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace PlayGround.Vision;

public class FilterOperationViewModel : ReactiveObject, IDisposable
{
    private bool _backingErrored;
    private readonly CompositeDisposable _compositeDisposable = new();
    public FilterOperationViewModel(IOperation operation, IOperationsService operationsService)
    {
        if (operationsService == null) throw new ArgumentNullException(nameof(operationsService));
        Id = operation.Id;
        Remove = ReactiveCommand.CreateFromTask(async () =>
        {
            await operationsService.RemoveWithId.Execute(Id);
        });
        Name = operation.GetType().Name;
        operation.Errored
            .Subscribe(error => Errored = error)
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