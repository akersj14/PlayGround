using System.Reactive;
using DynamicData;
using ReactiveUI;

namespace PlayGround.Vision;

public interface IListOfBlocks
{
    ReactiveCommand<IBlock, Unit> Add { get; }
    ReactiveCommand<int, Unit> RemoveWithId { get; }
    ReactiveCommand<(int startIndex, int targetIndex), Unit> Move { get; }
    IObservableList<IBlock> Operations { get; }
}

public class ListOfBlocks : IListOfBlocks
{
    private readonly SourceList<IBlock> _backingBlocks = new();

    public ListOfBlocks(ILiveFeedService liveFeedService, IOperationsService operationsService)
    {
        if (liveFeedService == null) 
            throw new ArgumentNullException(nameof(liveFeedService));
        if (operationsService == null) 
            throw new ArgumentNullException(nameof(operationsService));
        
        Operations = _backingBlocks.AsObservableList();
        var liveFeedBlock = new LiveFeedBlock(liveFeedService);
        var processedBlock = new ProcessedBlock();
        operationsService.Add.Execute(liveFeedBlock);
        operationsService.Add.Execute(processedBlock);
        
        Add = ReactiveCommand.Create<IBlock>(block =>
        {
            if (block.InputFieldNames.Count == 0 
                || block.OutputFieldNames.Count == 0
                || block.InputFieldNames.First() != "Image"
                || block.OutputFieldNames.First() != "Image") 
                return; 
            
            operationsService.Add.Execute(block);
            if (_backingBlocks.Count == 0)
            {
                operationsService.Connect.Execute((liveFeedBlock.Id, block.Id, "Image", "Image"));
            }
            else
            {
                var lastId = _backingBlocks.Items.Last().Id;
                operationsService.Connect.Execute((lastId, block.Id, "Image", "Image"));
            }
            _backingBlocks.Add(block);
        });
        //todo: implement RemoveWithId
        //todo: implement Move
        //todo: implement Operations
    }
    
    public ReactiveCommand<IBlock, Unit> Add { get; }
    public ReactiveCommand<int, Unit> RemoveWithId { get; }
    public ReactiveCommand<(int startIndex, int targetIndex), Unit> Move { get; }
    public IObservableList<IBlock> Operations { get; }
}