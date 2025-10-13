using CommunityToolkit.Mvvm.ComponentModel;
using PackEditor.Helpers;
using PackEditor.Models;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace PackEditor.ViewModels
{
    public partial class PreviewViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<BiomeCrop> biomeCrops = new();

        public void LoadCrops(WriteableBitmap sourceImage)
        {
            BiomeCrops.Clear();

            foreach (var region in BiomeRegions.All)
            {
                var cropped = new CroppedBitmap(sourceImage, region.Region);
                BiomeCrops.Add(new BiomeCrop
                {
                    Name = region.Name,
                    Crop = cropped
                });
            }
        }
    }
}
