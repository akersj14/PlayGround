using OpenCvSharp;

namespace PlayGround.Vision;

public interface IOperation
{
  Mat Operate(Mat mat);
}

public class Blur : IOperation
{
  public Mat Operate(Mat mat) => mat.Blur(new Size(100, 100));
}