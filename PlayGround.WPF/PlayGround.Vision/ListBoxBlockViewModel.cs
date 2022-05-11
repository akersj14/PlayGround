using System.Reactive;
using ReactiveUI;

namespace PlayGround.Vision;

public class ListBoxBlockViewModel : ReactiveObject
{
    private readonly ObservableAsPropertyHelper<bool> _backingErrored;
    public ListBoxBlockViewModel(int id, IOperationsService operationsService, IListOfBlocks listOfBlocks)
    {
        if (operationsService == null) 
            throw new ArgumentNullException(nameof(operationsService));
        var block = operationsService.Operations.Lookup(id);
        if (!block.HasValue)
            throw new ArgumentOutOfRangeException($"Block with id {id} does not exist");
        Block = block.Value;
        Name = block.Value.Title;
        Remove = ReactiveCommand.Create(() =>
        {
            listOfBlocks.RemoveBlockWithId(id);
        });
        _backingErrored = block.Value.Errored.ToProperty(this, nameof(Errored));
    }
    public string Name { get; }
    public IBlock Block { get; }
    public ReactiveCommand<Unit, Unit> Remove { get; }
    public bool Errored => _backingErrored.Value;
}