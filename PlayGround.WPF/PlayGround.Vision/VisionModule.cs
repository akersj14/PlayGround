using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PlayGround.Vision;

public class VisionModule
{
    private static ServiceProvider _serviceProvider;

    public VisionModule()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<LiveFeedViewModel>();
        serviceCollection.AddSingleton<ProcessedImageFeedViewModel>();
        serviceCollection.AddSingleton<ActiveBlockListViewModel>();
        serviceCollection.AddSingleton<IOperationsService, OperationsService>();
        serviceCollection.AddSingleton<ILiveFeedService,LiveFeedService>();
        serviceCollection.AddSingleton<IProcessedImageService, ProcessedImageService>();
        serviceCollection.AddSingleton<IVideoService, VideoService>();
        serviceCollection.AddSingleton<IListOfBlocks, ListOfBlocks>();
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