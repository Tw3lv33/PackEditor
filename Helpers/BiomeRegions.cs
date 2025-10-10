using System.Windows;

namespace PackEditor.Helpers
{
    //Static list of all biome positions
    public static class BiomeRegions
    {
        public static List<Models.BiomeRegion> All => new()
        {
            new Models.BiomeRegion { Name = "Forest", Region = new Int32Rect(0, 16, 16, 16)}, //Placeholder
        };
    }
}
