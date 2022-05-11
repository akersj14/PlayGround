using System.Reactive.Disposables;
using PlayGround.Vision;
using ReactiveUI;

namespace PlayGround.WPF;

public partial class ProcessedImageFeed 
{
    public ProcessedImageFeed()
    {
        InitializeComponent();
        ViewModel = VisionModule.Resolve<ProcessedImageFeedViewModel>();
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