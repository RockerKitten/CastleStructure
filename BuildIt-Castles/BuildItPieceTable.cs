// BuildIt-Castles
// a Valheim mod skeleton using Jötunn
// 
// File:    BuildIt-Castles.cs
// Project: BuildIt-Castles
using System.Collections.Generic;

namespace BuildItCastles
{
    public class BuildItPieceTable
    {
        public IEnumerable<BuildItPieceCategories> Categories { get; set; }
        public string TableName { get; set; }
    }
}