using ReactiveUI;

namespace PlayGround.Vision;

public class ListBoxBlockViewModel : ReactiveObject
{
    public ListBoxBlockViewModel(IBlock block, IListOfBlocks listOfBlocks)
    {
        Block = block;
        Id = block.Id;
        Name = block.Title;
    }
    public int Id { get; }
    public string Name { get; }
    public IBlock Block { get; }
}