using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackEditor.ViewModels;
using PackEditor.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PackEditor.Models
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            var dummyBitmap = new WriteableBitmap(256, 256, 96, 96, PixelFormats.Bgra32, null);
            var editorVM = new EditorViewModel { EditableImage = dummyBitmap };
            CurrentView = new EditorView(editorVM);
        }


        [ObservableProperty]
        private UserControl currentView;

        [ObservableProperty]
        private WriteableBitmap currentImage;

        [ObservableProperty]
        private string currentFileName;

        [ObservableProperty]
        private bool isPreviewAvailable;

        [RelayCommand]
        private void ShowEditor()
        {
            if (CurrentImage == null) return;

            var editorVM = new EditorViewModel
            {
                EditableImage = CurrentImage
            };

            CurrentView = new EditorView(editorVM);
        }


        [RelayCommand]
        private void ShowPreview() => CurrentView = new BiomePreviewView();

        [RelayCommand]
        private void LoadImage()
        {
            var result = Services.ImageLoader.LoadImage();
            if (result != null)
            {
                CurrentImage = result.Image;
                CurrentFileName = result.FileName;

                IsPreviewAvailable = result.FileName == "foliagecolor" || result.FileName == "grasscolor";

                ShowEditor(); // Load editor with the new image
            }
        }

        [RelayCommand]
        private void SaveAsPng()
        {
            if (CurrentImage != null)
            {
                var fileName = string.IsNullOrWhiteSpace(CurrentFileName) ? "Texture" : CurrentFileName;
                Services.ImageSaver.SaveAsPng(CurrentImage, fileName);
            }
        }

        [RelayCommand]
        private void Exit() 
        {
            System.Windows.Application.Current.Shutdown();
        }


    }
}
