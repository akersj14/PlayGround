using PlayGround.Core;
using System.Windows;

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
    base.OnStartup(e);
  }

  protected override void OnExit(ExitEventArgs e)
  {
    _containerProvider.Dispose();
    base.OnExit(e);
  }
}