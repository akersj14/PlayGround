using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;

namespace PlayGround.Vision;

public class ActiveBlockListViewModel : ReactiveObject, IDisposable
{
    private readonly ReadOnlyObservableCollection<ListBoxBlockViewModel> _backingFilterNames;
    private readonly CompositeDisposable _compositeDisposable = new();
    private readonly IListOfBlocks _listOfBlocks;

    public ActiveBlockListViewModel(IListOfBlocks listOfBlocks, IOperationsService operationsService)
    {
        if (operationsService == null) throw new ArgumentNullException(nameof(operationsService));
        _listOfBlocks = listOfBlocks ?? throw new ArgumentNullException(nameof(listOfBlocks));
        _listOfBlocks
            .Operations
            .Connect()
            .Distinct()
            .Transform(item => new ListBoxBlockViewModel(item, operationsService, _listOfBlocks))
            .Bind(out _backingFilterNames)
            .Subscribe()
            .DisposeWith(_compositeDisposable);
        
        OperationsToAdd = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IBlock).IsAssignableFrom(p) && typeof(IBlock) != p && typeof(BasicBlock) != p && typeof(BasicDisposableBlock) != p)
            .Select(item => new BlockToAddViewModel(item, _listOfBlocks))
            .ToList();
    }

    public ReadOnlyObservableCollection<ListBoxBlockViewModel> FilterNames => _backingFilterNames;

    public async Task MoveFilter(int sourceIndex, int targetIndex)
    {
        await _listOfBlocks.Move.Execute((sourceIndex, targetIndex));
    }

    public List<BlockToAddViewModel> OperationsToAdd { get; }

    public void Dispose() => _compositeDisposable.Dispose();
}