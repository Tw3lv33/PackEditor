using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PackEditor.Helpers
{
    //Biome Positions
    public static class BiomeRegions
    {
        public static List<Models.BiomeRegion> All => new()
        {
            new Models.BiomeRegion { Name = "Forest", Region = new Int32Rect(0, 16, 16, 16)}, //Placeholder
        };
    }
}
