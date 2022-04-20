using Microsoft.Extensions.DependencyInjection;
using PlayGround.Core.ViewModels;
using PlayGround.Vision;

namespace PlayGround.Core;

public class ContainerProvider
{
  private static ServiceProvider _serviceProvider;

  public ContainerProvider()
  {
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddSingleton<LiveFeedViewModel>();
    serviceCollection.AddSingleton<MainWindowViewModel>();
    serviceCollection.AddSingleton<IOperationsService, OperationsService>();
    serviceCollection.AddSingleton<IImageProcessor, ImageProcessor>();
    serviceCollection.AddSingleton<IVideoService, VideoService>();
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