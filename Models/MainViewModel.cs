using PackEditor.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackEditor.Views;
using System.Windows.Controls;

namespace PackEditor.Models
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            ShowEditorCommand = new RelayCommand(ShowEditor);
            ShowPreviewCommand = new RelayCommand(ShowPreview);
            CurrentView = new EditorView();
        }

        [ObservableProperty]
        private UserControl currentView;

        public IRelayCommand ShowEditorCommand { get; }
        public IRelayCommand ShowPreviewCommand { get; }

        private void ShowEditor() => CurrentView = new EditorView();
        private void ShowPreview() => CurrentView = new BiomePreviewView();
    }
}

