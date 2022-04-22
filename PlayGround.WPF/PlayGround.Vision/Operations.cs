using OpenCvSharp;

namespace PlayGround.Vision;

public interface IOperation
{
  Mat Operate(Mat mat);
  int Id { get; }
}

public class Blur : IOperation
{
  public Blur(int id) => Id = id;
  public Mat Operate(Mat mat) => mat.Blur(new Size(100, 100));
  public int Id { get; }
}

public class Erode : IOperation
{
  public Erode(int id) => Id = id;
  
  private InputArray inputArray = InputArray.KIND_MASK;
  public Mat Operate(Mat mat) => mat.Erode(inputArray);
  public int Id { get; }
}