using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using OpenCvSharp;

namespace PlayGround.Vision;

public interface IOperation
{
  Mat Operate(Mat mat);
  int Id { get; set; }
  IObservable<bool> Errored { get; }
}

public class KeepSame : IOperation
{
  private readonly Subject<bool> _backingErrored = new();
  public KeepSame()
  {
    Errored = _backingErrored.AsObservable();
    _backingErrored.OnNext(false);
  }
  public Mat Operate(Mat mat)
  {
    return mat.Clone();
  }

  public int Id { get; set; }
  public IObservable<bool> Errored { get; }
}
public class Blur : IOperation
{
  private readonly Subject<bool> _backingErrored = new();
  public Blur()
  {
    Errored = _backingErrored.AsObservable();
    _backingErrored.OnNext(false);
  }
  public Mat Operate(Mat mat)
  {
    return mat.Blur(new Size(100, 100));
  } 
  public int Id { get; set; }
  public IObservable<bool> Errored { get; }
}
public class GrayScale : IOperation
{
  private readonly Subject<bool> _backingErrored = new();
  public GrayScale()
  {
    Errored = _backingErrored.AsObservable();
    _backingErrored.OnNext(false);
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
  public IObservable<bool> Errored { get; }
}
public class Canny : IOperation
{
  private Subject<bool> _backingErrored = new();
  public Canny()
  {
    Errored = _backingErrored.AsObservable();
    _backingErrored.OnNext(false);
  }
  public Mat Operate(Mat mat)
  {
    try
    {
      var canny = mat.Canny(10, 100);
      _backingErrored.OnNext(false);
      return canny;
    }
    catch (Exception)
    {
      _backingErrored.OnNext(true);
      return mat.Clone();
    }
  }
  public int Id { get; set; }
  public IObservable<bool> Errored { get; }
}

public class Erode : IOperation
{
  private Subject<bool> _backingErrored = new();
  public Erode()
  {
    Errored = _backingErrored.AsObservable();
    _backingErrored.OnNext(false);
  }
  public Mat Operate(Mat mat)
  {
    var kernel = new [,] { {0,1,0}, {1,1,1},{0,1,0}};
    var kernelMat = new Mat(rows: 3, cols: 3, MatType.CV_8U, kernel);
    return mat.Erode(kernelMat, iterations: 100);
  }
  public int Id { get; set; }
  public IObservable<bool> Errored { get; }
}

public class Something : IOperation
{
  private Subject<bool> _backingErrored = new();
  public Something()
  {
    Errored = _backingErrored.AsObservable();
    _backingErrored.OnNext(false);
  }
  public Mat Operate(Mat mat)
  {
    try
    {
      var item = mat.Threshold(50, 255, ThresholdTypes.Binary);
      _backingErrored.OnNext(false);
      return item;
    }
    catch (Exception)
    {
      _backingErrored.OnNext(true);
      return mat.Clone();
    }
  }

  public int Id { get; set; }
  public IObservable<bool> Errored { get; }
}