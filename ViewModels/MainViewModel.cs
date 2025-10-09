using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackEditor.Views;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PackEditor.Models
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            CurrentView = new EditorView();
        }

        [ObservableProperty]
        private UserControl currentView;

        [ObservableProperty]
        private WriteableBitmap currentImage;

        [ObservableProperty]
        private string currentFileName;

        [RelayCommand]
        private void ShowEditor() => CurrentView = new EditorView();

        [RelayCommand]
        private void ShowPreview() => CurrentView = new BiomePreviewView();

        [RelayCommand]
        private void LoadImage()
        {
            var result = Services.ImageLoader.LoadImage();
            if (result != null)
            {
                currentImage = result.Image;
                currentFileName = result.FileName;
                ShowEditor();
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
