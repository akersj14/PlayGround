using System.Reflection;
using PlayGround.Core;
using System.Windows;
using ReactiveUI;
using Splat;

namespace PlayGround.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
  private ContainerProvider _containerProvider;

  protected override void OnStartup(StartupEventArgs e)
  {
    _containerProvider = new ContainerProvider();
    Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
    base.OnStartup(e);
  }

  protected override void OnExit(ExitEventArgs e)
  {
    _containerProvider.Dispose();
    base.OnExit(e);
  }

}