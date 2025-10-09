using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace PackEditor.Services
{
    public static class ImageSaver
    {
        public static void SaveAsPng(WriteableBitmap bitmap, string defaultFileName = "Texture")
        {
            if (bitmap == null) return;

            var dialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                DefaultExt = ".png",
                FileName = $"{defaultFileName}.png" 
            };

            if (dialog.ShowDialog() == true)
            {
                using var stream = new FileStream(dialog.FileName, FileMode.Create);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
            }
        }

    }
}
