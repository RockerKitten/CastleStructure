using System.Collections.Generic;

namespace BuildItCastles
{
    public class BuildItPiece
    {
        public string PrefabName { get; set; }
        public string DisplayNameToken { get; set; }
        public bool Enabled { get; set; } = true;

        public List<BuildItPieceRequirement> Requirements { get; set; }

        public BuildItMaterial Material { get; set; } = BuildItMaterial.Stone;

        public bool IsFire { get; set; } = false;

        public bool IsDoor { get; set; } = false;
    }
}
