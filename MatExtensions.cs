using System.IO;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace OpenCvSharp.Extensions
{
    public static class MatExtensions
    {
        public static BitmapSource ToBitmapSource(this Mat mat)
        {
            if (mat.Empty())
                return null;

            // Codifica o Mat como BMP (pode ser ".png" se preferir)
            byte[] imageData = mat.ImEncode(".bmp");
            using (var ms = new MemoryStream(imageData))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }
    }
}
