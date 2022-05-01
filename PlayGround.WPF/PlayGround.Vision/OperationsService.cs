using System.Reactive;
using DynamicData;
using ReactiveUI;

namespace PlayGround.Vision;

public interface IOperationsService : IDisposable
{
  IObservableCache<IBlock, int> Operations { get; }
  ReactiveCommand<int, Unit> RemoveAt { get; }
  ReactiveCommand<int, Unit> RemoveWithId { get; }
  ReactiveCommand<IBlock, Unit> Add { get; }
  ReactiveCommand<(int sourceId, int destinationId, string sourceFieldName, string destinationFieldName), bool> Connect { get; }
}

public class OperationsService : IOperationsService
{
  private readonly SourceCache<IBlock, int> _backingOperations = new(block => block.Id);
  private int _count;
  
  public OperationsService()
  {
    Operations = _backingOperations.AsObservableCache();
    
    Add = ReactiveCommand.Create<IBlock>(operation =>
    {
      operation.Id = _count++;
      _backingOperations.AddOrUpdate(operation);
    });

    RemoveAt = ReactiveCommand.Create<int>(position =>
    {
      if (position < 0 || position > _backingOperations.Count) return;
      _backingOperations.RemoveKey(position);
    });
    
    RemoveWithId = ReactiveCommand.Create<int>(id =>
    {
      try
      {
        var operationToRemove = _backingOperations.Items.First(item => item.Id == id);
        _backingOperations.RemoveKey(operationToRemove.Id);
      }
      catch (Exception e)
      {
        // ignored
      }
    });
    
    Connect = ReactiveCommand.Create<(int sourceId, int destinationId, string sourceFieldName, string destinationFieldName), bool>(tuple =>
    {
      var sourceOptional = Operations.Lookup(tuple.sourceId);
      var destinationOptional = Operations.Lookup(tuple.destinationId);
      if (!sourceOptional.HasValue || !destinationOptional.HasValue
                                   || !sourceOptional.Value.OutputFieldNames.Contains(tuple.sourceFieldName)
                                   || !destinationOptional.Value.InputFieldNames.Contains(tuple.sourceFieldName))
        return false;
      var sourcePropertyIndex = sourceOptional.Value.OutputFieldNames.IndexOf(tuple.sourceFieldName);
      if (sourceOptional.Value.Outputs.Count < sourcePropertyIndex) return false;
      
      return destinationOptional
        .Value
        .AddDependency(tuple.destinationFieldName, sourceOptional.Value, block => block.Outputs[sourcePropertyIndex]);
    });
  }

  public ReactiveCommand<IBlock, Unit> Add { get; }
  public ReactiveCommand<(int sourceId, int destinationId, string sourceFieldName, string destinationFieldName), bool> Connect { get; }
  public IObservableCache<IBlock, int> Operations { get; }
  public ReactiveCommand<int, Unit> RemoveAt { get; }
  public ReactiveCommand<int, Unit> RemoveWithId { get; }

  public void Dispose() => _backingOperations.Dispose();
}