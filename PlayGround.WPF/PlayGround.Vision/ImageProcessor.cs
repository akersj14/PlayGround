using OpenCvSharp;

namespace PlayGround.Vision;

public interface IImageProcessor
{
  Mat Process(Mat mat);
}

public class ImageProcessor : IImageProcessor
{
  private readonly IOperationsService _operationsService;

  public ImageProcessor(IOperationsService operationsService)
  {
    _operationsService = operationsService ?? throw new ArgumentNullException(nameof(operationsService));
  }

  public Mat Process(Mat mat)
  {
    
  }
}