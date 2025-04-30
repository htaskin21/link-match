using System.Collections.Generic;
using Chips;
using Cores;
using UnityEngine;

namespace Logic
{
    public interface IChipMatcher
    {
        Dictionary<Vector2Int, List<LinkableChip>> GenerateMatchCache(GridSystem<Chip> gridSystem);
    }
}