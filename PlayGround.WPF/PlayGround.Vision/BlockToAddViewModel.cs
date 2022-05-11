using System.Reactive;
using ReactiveUI;

namespace PlayGround.Vision;

public class BlockToAddViewModel
{
    public BlockToAddViewModel(Type type, IListOfBlocks listOfBlocks)
    {
        if (!type.IsAssignableTo(typeof(IBlock)))
        {
            Name = $"Not a Block {type.FullName}";
            Add = ReactiveCommand.Create<Unit>(_ => { });
            return;
        }
        
        var block = (IBlock)Activator.CreateInstance(type);
        Name = block.Title;
        Add = ReactiveCommand.Create<Unit>(_ =>
        {
            listOfBlocks.AddBlock(block);
        });
    }
    public ReactiveCommand<Unit, Unit> Add { get; }
    public string Name { get; }
}