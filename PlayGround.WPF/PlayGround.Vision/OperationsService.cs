using System.Reactive;
using DynamicData;
using ReactiveUI;

namespace PlayGround.Vision;

public interface IOperationsService : IDisposable
{
  IObservableList<IOperation> Operations { get; }
  ReactiveCommand<int, Unit> RemoveAt { get; }
  ReactiveCommand<int, Unit> RemoveWithId { get; }
  ReactiveCommand<IOperation, Unit> Add { get; }
  ReactiveCommand<(int, int), Unit> Move { get; }
}

public class OperationsService : IOperationsService
{
  private readonly SourceList<IOperation> _backingOperations = new();
  private int _count;
  public OperationsService()
  {
    Operations = _backingOperations.AsObservableList();
    Add = ReactiveCommand.Create<IOperation>(operation =>
    {
      operation.Id = _count++;
      _backingOperations.Add(operation);
    });

    RemoveAt = ReactiveCommand.Create<int>(position =>
    {
      if (position < 0 || position > _backingOperations.Count) return;
        _backingOperations.RemoveAt(position);
    });
    RemoveWithId = ReactiveCommand.Create<int>(id =>
    {
      try
      {
        var operationToRemove = _backingOperations.Items.First(item => item.Id == id);
        _backingOperations.Remove(operationToRemove);
      }
      catch (Exception e)
      {
        // ignored
      }
    });
    Move = ReactiveCommand.Create<(int sourceIndex, int targetIndex)>(tuple =>
    {
      if (tuple.sourceIndex < 0 || tuple.sourceIndex > _backingOperations.Count 
                               || tuple.targetIndex < 0 || tuple.targetIndex > _backingOperations.Count)
        return;
      _backingOperations.Move(tuple.sourceIndex, tuple.targetIndex);

    });
  }

  public ReactiveCommand<IOperation, Unit> Add { get; }
  public ReactiveCommand<(int, int), Unit> Move { get; }
  public IObservableList<IOperation> Operations { get; }
  public ReactiveCommand<int, Unit> RemoveAt { get; }
  public ReactiveCommand<int, Unit> RemoveWithId { get; }

  public void Dispose()
  {
    _backingOperations.Dispose();
  }
}