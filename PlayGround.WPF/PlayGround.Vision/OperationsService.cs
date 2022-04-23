using System.Linq.Expressions;
using DynamicData;

namespace PlayGround.Vision;

public interface IOperationsService : IDisposable
{
  IObservableList<IOperation> Operations { get; }
  void RemoveAt(int position);
  void RemoveWithId(int id);
  void Add(IOperation operation);
  void Move(int sourceIndex, int targetIndex);
}

public class OperationsService : IOperationsService
{
  private readonly SourceList<IOperation> _backingOperations = new();
  private int _count;
  public OperationsService()
  {
    Add(new KeepSame());
    Operations = _backingOperations.AsObservableList();
  }

  public void RemoveAt(int position)
  {
    if (position < 0 || position > _backingOperations.Count) return;
    _backingOperations.RemoveAt(position);
  }

  public void RemoveWithId(int id)
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
  }

  public void Add(IOperation operation)
  {
    operation.Id = _count++;
    _backingOperations.Add(operation);
  }

  public void Move(int sourceIndex, int targetIndex)
  {
    if (sourceIndex < 0 || sourceIndex > _backingOperations.Count 
                        || targetIndex < 0 || targetIndex > _backingOperations.Count)
      return;
    _backingOperations.Move(sourceIndex, targetIndex);
  }
  
  public IObservableList<IOperation> Operations { get; }
  public void Dispose()
  {
    _backingOperations.Dispose();
  }
}