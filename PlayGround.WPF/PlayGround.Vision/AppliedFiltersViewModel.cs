using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using DynamicData;
using ReactiveUI;

namespace PlayGround.Vision;

public class AppliedFiltersViewModel : ReactiveObject, IDisposable
{
    private readonly ReadOnlyObservableCollection<FilterOperationViewModel> _backingFilterNames;
    private readonly CompositeDisposable _compositeDisposable = new();
    private readonly IOperationsService _operationsService;
    public AppliedFiltersViewModel(IOperationsService operationsService)
    {
        _operationsService = operationsService ?? throw new ArgumentNullException(nameof(operationsService));
        operationsService
            .Operations
            .Connect()
            .Transform(item => new FilterOperationViewModel(item, operationsService))
            .Bind(out _backingFilterNames)
            .Subscribe()
            .DisposeWith(_compositeDisposable);
        
        OperationsToAdd =  AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IOperation).IsAssignableFrom(p) && typeof(IOperation) != p)
            .Select(item => new OperationToAddViewModel(item, operationsService))
            .ToList();
    }

    public ReadOnlyObservableCollection<FilterOperationViewModel> FilterNames => _backingFilterNames;

    public void MoveFilter(int sourceIndex, int targetIndex)
    {
        _operationsService.Move(sourceIndex, targetIndex);
    }

    public List<OperationToAddViewModel> OperationsToAdd { get; }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}