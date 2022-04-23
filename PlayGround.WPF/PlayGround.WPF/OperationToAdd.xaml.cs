using System.Reactive.Disposables;
using PlayGround.Vision;
using ReactiveUI;

namespace PlayGround.WPF;

public partial class OperationToAdd 
{
    public OperationToAdd()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            ViewModel = (OperationToAddViewModel) DataContext;

            this.BindCommand(ViewModel,
                    model => model.Add,
                    view => view.SelectedButton)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel,
                    model => model.Content,
                    view => view.SelectedButton.Content)
                .DisposeWith(disposable);
        });
    }
}