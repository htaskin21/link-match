using System.Collections.Generic;
using Chips;
using Cores;
using UnityEngine;

namespace Logic
{
    // Defines contract for classes that generate match caches from a grid.
    public interface IChipMatcher
    {
        Dictionary<Vector2Int, List<LinkableChip>> GenerateMatchCache(GridSystem<Chip> gridSystem);
    }
}