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
    Add(new Blur(_count++));
    Add(new Erode(_count++));
    Add(new Blur(_count++));
    Add(new Erode(_count++));
    Operations = _backingOperations.AsObservableList();
  }

  public void RemoveAt(int position)
  {
    _backingOperations.RemoveAt(position);
  }

  public void RemoveWithId(int id)
  {
    var operationToRemove = _backingOperations.Items.First(item => item.Id == id);
    _backingOperations.Remove(operationToRemove);
  }

  public void Add(IOperation operation)
  {
    _backingOperations.Add(operation);
  }

  public void Move(int sourceIndex, int targetIndex)
  {
    if (sourceIndex < 0 || sourceIndex > _backingOperations.Count 
                        || targetIndex < 0 || targetIndex > _backingOperations.Count)
      return;
    
    var item = _backingOperations.Items.ElementAt(sourceIndex);
    _backingOperations.RemoveAt(sourceIndex);
    _backingOperations.Insert(targetIndex, item);
  }
  
  public IObservableList<IOperation> Operations { get; }
  public void Dispose()
  {
    _backingOperations.Dispose();
  }
}