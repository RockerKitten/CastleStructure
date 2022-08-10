using System.Collections.Generic;

namespace BuildItCastles
{
    public class BuildItPiece
    {
        public string PrefabName { get; set; }
        public string PrefabDescription { get; set; } = string.Empty;
        public string DisplayNameToken { get; set; }
        public bool Enabled { get; set; } = true;
        public string RequiredStation { get; set; } = string.Empty;

        public List<BuildItPieceRequirement> Requirements { get; set; }

        public BuildItMaterial Material { get; set; } = BuildItMaterial.Stone;

        public string FuelItem { get; set; }
    }
}
