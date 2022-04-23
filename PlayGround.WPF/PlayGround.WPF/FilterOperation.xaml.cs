using System.Reactive.Disposables;
using PlayGround.Vision;
using ReactiveUI;

namespace PlayGround.WPF;

public partial class FilterOperation 
{
    public FilterOperation()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            ViewModel = (FilterOperationViewModel) DataContext;

            this.OneWayBind(ViewModel,
                    model => model.Name,
                    view => view.MainLabel.Content)
                .DisposeWith(disposable);

            this.BindCommand(ViewModel,
                    model => model.Remove,
                    view => view.RemoveButton)
                .DisposeWith(disposable);
            this.OneWayBind(ViewModel,
                    model => model.Errored,
                    view => view.RedDot.Visibility)
                .DisposeWith(disposable);
        });
    }
}