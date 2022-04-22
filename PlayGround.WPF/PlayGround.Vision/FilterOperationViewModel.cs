using System.Reactive;
using ReactiveUI;

namespace PlayGround.Vision;

public class FilterOperationViewModel : ReactiveObject
{
    public FilterOperationViewModel(IOperation operation, IOperationsService operationsService)
    {
        if (operationsService == null) throw new ArgumentNullException(nameof(operationsService));
        Id = operation.Id;
        Remove = ReactiveCommand.Create(() =>
        {
            operationsService.RemoveWithId(Id);
        });
        Name = operation.GetType().Name;
    }
    public int Id { get; }
    public ReactiveCommand<Unit, Unit> Remove;
    public string Name { get; }
}