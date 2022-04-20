using ReactiveUI;
using System.Reactive.Disposables;

namespace PlayGround.Vision.WPF
{
  /// <summary>
  /// Interaction logic for LiveFeed.xaml
  /// </summary>
  public partial class LiveFeed
  {
    public LiveFeed()
    {
      InitializeComponent();
      ViewModel = ();
      DataContext = ViewModel;

      this.WhenActivated(disposable =>
      {
        this.OneWayBind(ViewModel,
            model => model.Image,
            feed => feed.Image.Source)
          .DisposeWith(disposable);
      });
    }
  }
}
