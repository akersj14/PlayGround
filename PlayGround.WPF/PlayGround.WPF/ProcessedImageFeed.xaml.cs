using System.Reactive.Disposables;
using System.Windows.Controls;
using PlayGround.Core;
using PlayGround.Vision;
using ReactiveUI;

namespace PlayGround.WPF;

public partial class ProcessedImageFeed 
{
    public ProcessedImageFeed()
    {
        InitializeComponent();
        ViewModel = ContainerProvider.Resolve<ProcessedImageFeedViewModel>();
        DataContext = ViewModel;
        
        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel,
                    model => model.ProcessedImage,
                    feed => feed.ProcessedImage.Source)
                .DisposeWith(disposable);
        });
    }
}