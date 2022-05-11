using System.Reflection;
using PlayGround.Core;
using System.Windows;
using PlayGround.Vision;
using ReactiveUI;
using Splat;

namespace PlayGround.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
  private ContainerProvider _containerProvider;
  private VisionModule _visionModule;

  protected override void OnStartup(StartupEventArgs e)
  {
    _containerProvider = new ContainerProvider();
    _visionModule = new VisionModule();
    Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
    base.OnStartup(e);
  }

  protected override void OnExit(ExitEventArgs e)
  {
    _containerProvider.Dispose();
    _visionModule.Dispose();
    base.OnExit(e);
  }
}