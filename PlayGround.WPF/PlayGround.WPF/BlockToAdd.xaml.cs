using System.Reactive.Disposables;
using PlayGround.Vision;
using ReactiveUI;

namespace PlayGround.WPF;

public partial class BlockToAdd 
{
    public BlockToAdd()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            ViewModel = (BlockToAddViewModel) DataContext;

            this.OneWayBind(ViewModel,
                    model => model.Name,
                    view => view.TitleButton.Content)
                .DisposeWith(disposable);
            
            this.BindCommand(ViewModel,
                    model => model.Add,
                    view => view.TitleButton)
                .DisposeWith(disposable);
        });
    }
}