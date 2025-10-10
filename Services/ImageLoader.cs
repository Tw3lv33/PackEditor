using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace PackEditor.Services
{
    public static class ImageLoader
    {
        //Image to edit and its filename without extension
        public class ImageResult
        {
            public WriteableBitmap Image { get; set; }
            public string FileName { get; set; }
        }

        //Open file dialog to load a PNG image and return it as a WriteableBitmap along with its filename
        public static ImageResult LoadImage()
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "PNG Images (*.png)|*.png",
                Title = "Choose the texture"
            };

            if (dialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage();
                using (var stream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    bitmap.Freeze();
                }

                return new ImageResult
                {
                    Image = new WriteableBitmap(bitmap),
                    FileName = Path.GetFileNameWithoutExtension(dialog.FileName)
                };
            }

            return null;
        }
    }
}
