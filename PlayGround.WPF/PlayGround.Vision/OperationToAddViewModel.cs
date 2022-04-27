using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace PlayGround.Vision;

public class OperationToAddViewModel : ReactiveObject
{
    public OperationToAddViewModel(Type operationType, IOperationsService operationsService)
    {
        IOperation operation;
        if (typeof(IOperation).IsAssignableFrom(operationType))
            operation = (IOperation) Activator.CreateInstance(operationType);
        else
        {
            operation = new KeepSame();
        }
        
        if (operationsService == null) 
            throw new ArgumentNullException(nameof(operationsService));
        Add = ReactiveCommand.CreateFromTask(async () =>
        {
            await operationsService.Add.Execute(operation);
        });
        Content = operation.GetType().Name;
    }

    public ReactiveCommand<Unit, Unit> Add;
    public string Content { get; }
}