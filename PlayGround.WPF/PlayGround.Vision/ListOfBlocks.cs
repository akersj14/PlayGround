using System.Reactive;
using DynamicData;
using ReactiveUI;

namespace PlayGround.Vision;

public interface IListOfBlocks
{
    ReactiveCommand<IBlock, Unit> Add { get; }
    ReactiveCommand<int, Unit> RemoveWithId { get; }
    void RemoveBlockWithId(int id);
    void MoveBlock(int startIndex, int targetIndex);
    void AddBlock(IBlock block);
    ReactiveCommand<(int startIndex, int targetIndex), Unit> Move { get; }
    IObservableList<int> Operations { get; }
}

public class ListOfBlocks : IListOfBlocks
{
    private readonly SourceList<int> _backingBlocks = new();
    private readonly LiveFeedBlock _liveFeedBlock;
    private readonly ProcessedBlock _processedBlock;
    private readonly IOperationsService _operationsService;
    
    public ListOfBlocks(IOperationsService operationsService)
    {
        _operationsService = operationsService ?? throw new ArgumentNullException(nameof(operationsService));

        Operations = _backingBlocks.AsObservableList();
        _liveFeedBlock = new LiveFeedBlock();
        _processedBlock = new ProcessedBlock();
        operationsService.AddBlock(_liveFeedBlock);
        operationsService.AddBlock(_processedBlock);
        operationsService.ConnectBlocks(_liveFeedBlock.Id, _processedBlock.Id, "Image", "Image");
        
        Add = ReactiveCommand.Create<IBlock>(AddBlock);
        RemoveWithId = ReactiveCommand.Create<int>(RemoveBlockWithId);
        Move = ReactiveCommand.Create<(int startIndex, int targetIndex)>(tuple =>
        {
            MoveBlock(tuple.startIndex, tuple.targetIndex);
        });
    }
    
    public ReactiveCommand<IBlock, Unit> Add { get; }
    public ReactiveCommand<int, Unit> RemoveWithId { get; }
    public void RemoveBlockWithId(int id)
    {
        if (!_backingBlocks.Items.Contains(id)) return;
            
        var indexToRemove = _backingBlocks.Items.IndexOf(id);
        var indexBefore = indexToRemove - 1;
        var indexAfter = indexToRemove + 1;

        IBlock blockBefore;
        IBlock blockAfter;

        if (indexBefore == -1)
        {
            blockBefore = _liveFeedBlock;
        }
        else
        {
            var optionalBlockBefore = _operationsService.Operations.Lookup(indexBefore);
            if (!optionalBlockBefore.HasValue) return;
            blockBefore = optionalBlockBefore.Value;
        }

        if (indexAfter == _backingBlocks.Count)
        {
            blockAfter = _processedBlock;
        }
        else
        {
            var optionalBlockAfter = _operationsService.Operations.Lookup(indexAfter);
            if (!optionalBlockAfter.HasValue) return;
            blockAfter = optionalBlockAfter.Value;
        }

        _operationsService.ConnectBlocks(blockBefore.Id, blockAfter.Id, "Image", "Image");
        _backingBlocks.Remove(id);
        _operationsService.RemoveBlockWithId(id);
    }

    public void MoveBlock(int startIndex, int targetIndex)
    {
        if (startIndex < 0 || targetIndex < 0
                                 || startIndex >= _backingBlocks.Count
                                 || targetIndex >= _backingBlocks.Count
                                 || targetIndex == startIndex)
            return;
        var blocksList = _backingBlocks.Items.ToList();
        var movingBlock = _operationsService.Operations.Lookup(blocksList[startIndex]).Value;
            
        var beforeStartBlock = startIndex > 0
            ? _operationsService.Operations.Lookup(blocksList[startIndex - 1]).Value
            : _liveFeedBlock;
        var afterStartBlock = startIndex < blocksList.Count - 1
            ? _operationsService.Operations.Lookup(blocksList[startIndex + 1]).Value
            : _processedBlock;
        var beforeTargetBlock = targetIndex > 0
            ? _operationsService.Operations.Lookup(blocksList[targetIndex - 1]).Value
            : _liveFeedBlock;
        var afterTargetBlock = targetIndex < blocksList.Count - 1
            ? _operationsService.Operations.Lookup(blocksList[targetIndex]).Value
            : _processedBlock;
            
        _backingBlocks.Move(startIndex, targetIndex);
            
        _operationsService.ConnectBlocks(beforeStartBlock.Id, afterStartBlock.Id, "Image", "Image");
        _operationsService.ConnectBlocks(beforeTargetBlock.Id, movingBlock.Id, "Image", "Image");
        _operationsService.ConnectBlocks(movingBlock.Id, afterTargetBlock.Id, "Image", "Image");
    }

    public void AddBlock(IBlock block)
    {
        if (block.InputFieldNames.Count == 0 
            || block.OutputFieldNames.Count == 0
            || block.InputFieldNames.First() != "Image"
            || block.OutputFieldNames.First() != "Image") 
            return; 
            
        _operationsService.AddBlock(block);
        if (_backingBlocks.Count == 0)
        {
            _operationsService.ConnectBlocks(_liveFeedBlock.Id, block.Id, "Image", "Image");
        }
        else
        {
            var lastId = _backingBlocks.Items.Last();
            _operationsService.ConnectBlocks(lastId, block.Id, "Image", "Image");
        }
        _backingBlocks.Add(block.Id);
    }

    public ReactiveCommand<(int startIndex, int targetIndex), Unit> Move { get; }
    public IObservableList<int> Operations { get; }
}