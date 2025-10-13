using System.IO;
using System.IO.Compression;
using System.Windows.Media.Imaging;

namespace PackEditor.Services
{
    public static class ZipExporter
    {
        // Export the WriteableBitmap as a PNG inside a ZIP archive
        public static void ExportAsZip(WriteableBitmap bitmap, string imageName = "Texture")
        {
            if (bitmap == null) return;

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "ZIP Archive|*.zip",
                DefaultExt = ".zip",
                FileName = $"{imageName}.zip"
            };

            if (dialog.ShowDialog() == true)
            {
                using var zipStream = new FileStream(dialog.FileName, FileMode.Create);
                using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create);

                // Normalize name for comparison
                string normalizedName = imageName.ToLowerInvariant();
                string folder = (normalizedName == "foliagecolor" || normalizedName == "grasscolor") ? "misc/" : "";

                var entry = archive.CreateEntry($"{folder}{imageName}.png");

                using var entryStream = entry.Open();
                using var memoryStream = new MemoryStream();

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(entryStream);
            }
        }
    }
}
