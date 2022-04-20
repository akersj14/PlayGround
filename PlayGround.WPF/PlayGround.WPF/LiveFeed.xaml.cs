using ReactiveUI;
using System.Reactive.Disposables;
using PlayGround.Core;
using PlayGround.Vision;

namespace PlayGround.WPF;

/// <summary>
/// Interaction logic for LiveFeed.xaml
/// </summary>
public partial class LiveFeed
{
  public LiveFeed()
  {
    InitializeComponent();
    ViewModel = ContainerProvider.Resolve<LiveFeedViewModel>();
    DataContext = ViewModel;

    this.WhenActivated(disposable =>
    {
      this.OneWayBind(ViewModel,
          model => model.OriginalImage,
          feed => feed.OriginalImage.Source)
        .DisposeWith(disposable);
    });
  }
}