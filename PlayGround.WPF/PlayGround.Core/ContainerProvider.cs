using Microsoft.Extensions.DependencyInjection;
using PlayGround.Core.ViewModels;

namespace PlayGround.Core;

public class ContainerProvider
{
  private static ServiceProvider _serviceProvider;

  public ContainerProvider()
  {
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddSingleton<MainWindowViewModel>();
    serviceCollection.AddTransient<DragDropCanvasViewModel>();
    serviceCollection.AddTransient<DroppableObjectViewModel>();
    _serviceProvider = serviceCollection.BuildServiceProvider();
  }

  public static T Resolve<T>()
  {
    return _serviceProvider.GetService<T>()!;
  }

  public void Dispose()
  {
    _serviceProvider.Dispose();
  }
}