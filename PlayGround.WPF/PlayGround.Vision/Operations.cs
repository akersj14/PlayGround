using System.Reactive.Disposables;
using System.Reactive.Subjects;
using OpenCvSharp;

namespace PlayGround.Vision;

public interface IOperation
{
  Mat Operate(Mat mat);
  int Id { get; set; }
  bool Errored { get; }
}

public class KeepSame : IOperation
{
  public Mat Operate(Mat mat)
  {
    return mat.Clone();
  }

  public int Id { get; set; }
  public bool Errored => false;
}
public class Blur : IOperation
{
  public Mat Operate(Mat mat)
  {
    return mat.Blur(new Size(100, 100));
  } 
  public int Id { get; set; }
  public bool Errored => false;
}
public class GrayScale : IOperation, IDisposable
{
  private readonly Subject<bool> _backingErrored = new();
  private readonly CompositeDisposable _compositeDisposable = new();
  public GrayScale()
  {
    _backingErrored.Subscribe(error => Errored = error).DisposeWith(_compositeDisposable);
  }
  public Mat Operate(Mat mat)
  {
    try
    {
      var gray = mat.CvtColor(ColorConversionCodes.BGRA2GRAY);
      _backingErrored.OnNext(false);
      return gray;
    }
    catch (Exception)
    {
      _backingErrored.OnNext(true);
      return mat.Clone();
    }
  }

  public int Id { get; set; }
  public bool Errored { get; private set; }
  public void Dispose() => _compositeDisposable.Dispose();
}
public class Canny : IOperation
{
  public Mat Operate(Mat mat)
  {
    try
    {
      var canny = mat.Canny(10, 100);
      Errored = false;
      return canny;
    }
    catch (Exception)
    {
      return mat.Clone();
    }
  }
  public int Id { get; set; }
  public bool Errored { get; private set; }
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
  public bool Errored => false;
}