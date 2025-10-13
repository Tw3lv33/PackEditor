using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackEditor.ViewModels;
using PackEditor.Views;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PackEditor.Models
{
    public partial class MainViewModel : ObservableObject
    {
        // Initialize with a dummy image for the editor view
        public MainViewModel()
        {
            var dummyBitmap = new WriteableBitmap(256, 256, 96, 96, PixelFormats.Bgra32, null);
            var editorVM = new EditorViewModel { EditableImage = dummyBitmap };
            CurrentView = new EditorView(editorVM);
        }

        // Current view being displayed (Editor or Preview)
        [ObservableProperty]
        private UserControl currentView;

        // Currently loaded image
        [ObservableProperty]
        public WriteableBitmap currentImage;

        // Filename of the currently loaded image
        [ObservableProperty]
        private string currentFileName;

        // Whether the preview option is available based on the loaded image
        [ObservableProperty]
        private bool isPreviewAvailable;

        // Command to switch to the editor view
        [RelayCommand]
        private void ShowEditor()
        {
            if (CurrentImage == null) return;

            var editorVM = new EditorViewModel
            {
                EditableImage = CurrentImage,
                OnImageChanged = updatedImage => CurrentImage = updatedImage
            };
            editorVM.InitializeHistory(); // Push initial image to history
            CurrentView = new EditorView(editorVM);

        }


        // Command to switch to the biome preview view
        [RelayCommand]
        private void ShowPreview()
        {
            if (CurrentImage == null) return;

            var previewVM = new PreviewViewModel();
            previewVM.LoadCrops(CurrentImage);

            var previewView = new BiomePreviewView
            {
                DataContext = previewVM
            };

            CurrentView = previewView;
        }

        // Command to load an image using the ImageLoader service
        [RelayCommand]
        private void LoadImage()
        {
            var result = Services.ImageLoader.LoadImage();
            if (result != null)
            {
                CurrentImage = result.Image;
                CurrentFileName = result.FileName;

                IsPreviewAvailable = result.FileName == "foliagecolor" || result.FileName == "grasscolor";

                ShowEditor();
            }
        }

        // Command to save the current image as a PNG using the ImageSaver service
        [RelayCommand]
        private void SaveAsPng()
        {
            if (CurrentImage != null)
            {
                var fileName = string.IsNullOrWhiteSpace(CurrentFileName) ? "Texture" : CurrentFileName;
                Services.ImageSaver.SaveAsPng(CurrentImage, fileName);
            }
        }

        // Command to export the current image as a PNG inside a ZIP using the ZipExporter service
        [RelayCommand]
        private void ExportAsZip()
        {
            if (CurrentImage != null)
            {
                var fileName = string.IsNullOrWhiteSpace(CurrentFileName) ? "Texture" : CurrentFileName;
                Services.ZipExporter.ExportAsZip(CurrentImage, fileName);
            }
        }


        // Command to exit the application
        [RelayCommand]
        private void Exit() 
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
