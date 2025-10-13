using PackEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PackEditor.Views
{
    /// <summary>
    /// Logika interakcji dla klasy BiomePreviewView.xaml
    /// </summary>
    public partial class BiomePreviewView : UserControl
    {
        public PreviewViewModel ViewModel { get; } = new();

        public BiomePreviewView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
