using OpenCvSharp;

namespace PlayGround.Vision;

public interface IOperation
{
  Mat Operate(Mat mat);
  int Id { get; set; }
}

public class KeepSame : IOperation
{
  public Mat Operate(Mat mat)
  {
    return mat.Clone();
  }

  public int Id { get; set; }
}
public class Blur : IOperation
{
  public Mat Operate(Mat mat)
  {
    return mat.Blur(new Size(100, 100));
  } 
  public int Id { get; set; }
}
public class GrayScale : IOperation
{
  public Mat Operate(Mat mat)
  {
    try
    {
      var gray = mat.CvtColor(ColorConversionCodes.BGRA2GRAY);
      return gray;
    }
    catch (Exception)
    {
      return mat.Clone();
    }
  }

  public int Id { get; set; }
}
public class Canny : IOperation
{
  public Mat Operate(Mat mat)
  {
    try
    {
      return mat.Canny(10, 100);
    }
    catch (Exception)
    {
      return mat.Clone();
    }
  }
  public int Id { get; set; }
}

public class Erode : IOperation
{
  public Mat Operate(Mat mat)
  {
    var kernel = new [,] { {0,1,0}, {1,1,1},{0,1,0}};
    var kernelMat = new Mat(rows: 3, cols: 3, MatType.CV_8U, kernel);
    return mat.Erode(kernelMat, iterations: 100);
  }
  public int Id { get; set; }
}