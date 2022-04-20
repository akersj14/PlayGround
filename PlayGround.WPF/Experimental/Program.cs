using OpenCvSharp;

namespace Experimental;

internal class Program
{
  private static void Main(string[] args)
  {
    var capture = new VideoCapture(0);
    using var window = new Window("Webcam");
    using var image = new Mat();
    while (true)
    {
      capture.Read(image);
      if (image.Empty()) break;
      window.ShowImage(image);
      var key = Cv2.WaitKey(30);
      if (key == 27) break;
    }
  }
}