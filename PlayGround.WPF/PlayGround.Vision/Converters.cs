using System.Drawing;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PlayGround.Vision;

public static class Converters
{
    public static BitmapImage BitmapToImageSource(Image bitmap)
    {
        using var memory = new MemoryStream();
        bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
        memory.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memory;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return bitmapImage;
    }

    public static BitmapImage MatToBitmapImage(Mat mat)
    {
        var bitmap = mat.ToBitmap();
        using var memory = new MemoryStream();
        bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
        memory.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memory;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return bitmapImage;
    }
}