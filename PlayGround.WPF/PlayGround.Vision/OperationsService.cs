using System.Reactive;
using DynamicData;
using DynamicData.Kernel;
using ReactiveUI;

namespace PlayGround.Vision;

public interface IOperationsService : IDisposable
{
  IObservableCache<IBlock, int> Operations { get; }
  ReactiveCommand<int, Unit> RemoveAt { get; }
  ReactiveCommand<int, Unit> RemoveWithId { get; }
  ReactiveCommand<IBlock, Unit> Add { get; }
  ReactiveCommand<(int sourceId, int destinationId, string sourceFieldName, string destinationFieldName), bool> Connect { get; }
  void AddBlock(IBlock block);
  void RemoveBlockWithId(int id);
  bool ConnectBlocks(int sourceId, int destinationId, string sourceFieldName, string destinationFieldName);
}

public class OperationsService : IOperationsService
{
  private readonly SourceCache<IBlock, int> _backingOperations = new(block => block.Id);
  private int _count;
  
  public OperationsService()
  {
    Operations = _backingOperations.AsObservableCache();
    
    Add = ReactiveCommand.Create<IBlock>(AddBlock);

    RemoveAt = ReactiveCommand.Create<int>(position =>
    {
      if (position < 0 || position > _backingOperations.Count) return;
      _backingOperations.RemoveKey(position);
    });
    
    RemoveWithId = ReactiveCommand.Create<int>(RemoveBlockWithId);
    
    Connect = ReactiveCommand
      .Create<(int sourceId, int destinationId, string sourceFieldName, string destinationFieldName), bool>(tuple 
        => ConnectBlocks(tuple.sourceId, tuple.destinationId, tuple.sourceFieldName, tuple.destinationFieldName));
  }

  public ReactiveCommand<IBlock, Unit> Add { get; }
  public ReactiveCommand<(int sourceId, int destinationId, string sourceFieldName, string destinationFieldName), bool> Connect { get; }
  public IObservableCache<IBlock, int> Operations { get; }
  public ReactiveCommand<int, Unit> RemoveAt { get; }
  public ReactiveCommand<int, Unit> RemoveWithId { get; }

  public void AddBlock(IBlock block)
  {
    block.Id = _count++;
    _backingOperations.AddOrUpdate(block);
  }

  public void RemoveBlockWithId(int id)
  {
      var operationToRemove = _backingOperations.Items.FirstOrOptional(item => item.Id == id);
      if (operationToRemove.HasValue)
        _backingOperations.RemoveKey(operationToRemove.Value.Id);
  }

  public bool ConnectBlocks(int sourceId, int destinationId, string sourceFieldName, string destinationFieldName)
  {
    var sourceOptional = Operations.Lookup(sourceId);
    var destinationOptional = Operations.Lookup(destinationId);
    if (!sourceOptional.HasValue || !destinationOptional.HasValue
                                 || !sourceOptional.Value.OutputFieldNames.Contains(sourceFieldName)
                                 || !destinationOptional.Value.InputFieldNames.Contains(sourceFieldName))
      return false;
    var sourcePropertyIndex = sourceOptional.Value.OutputFieldNames.IndexOf(sourceFieldName);
    if (sourceOptional.Value.Outputs.Count < sourcePropertyIndex) 
      return false;
    
    return destinationOptional
      .Value
      .AddDependency(destinationFieldName, sourceOptional.Value, block => block.Outputs[sourcePropertyIndex]);
    
  }

  public void Dispose() => _backingOperations.Dispose();
}