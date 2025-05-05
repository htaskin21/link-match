using System.Collections.Generic;
using System.Linq;
using Chips;
using Cores;
using UnityEngine;

namespace Matchers
{
    // Finds groups of connected same-color chips in four directions and caches matches.
    public class FourWayChipMatcher : IChipMatcher
    {
        private readonly Vector2Int[] _directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        private readonly int _minMatchCount;

        public FourWayChipMatcher(int minMatchCount = 3)
        {
            _minMatchCount = minMatchCount;
        }

        /// <summary>
        /// Generates a map of matched chip groups keyed by each chip's position.
        /// </summary>
        public Dictionary<Vector2Int, List<LinkableChip>> GenerateMatchCache(GridSystem<Chip> gridSystem)
        {
            var matchCache = new Dictionary<Vector2Int, List<LinkableChip>>();
            var visited = new HashSet<Vector2Int>();

            for (var y = 0; y < gridSystem.GridSize.y; y++)
            {
                for (var x = 0; x < gridSystem.GridSize.x; x++)
                {
                    Vector2Int pos = new(x, y);
                    if (!gridSystem.CheckBounds(pos) || visited.Contains(pos) || gridSystem.IsEmpty(pos))
                        continue;

                    if (gridSystem.GetItemAt(pos) is not LinkableChip startChip)
                        continue;

                    var group = FindConnectedChips(startChip, gridSystem);

                    if (group.Count < _minMatchCount)
                        continue;

                    foreach (var chip in group.Where(chip => !visited.Contains(chip.Position)))
                    {
                        matchCache[chip.Position] = group;
                        visited.Add(chip.Position);
                    }
                }
            }

            return matchCache;
        }

        /// <summary>
        /// Performs depth-first search to collect all same-color chips connected to startChip.
        /// </summary>
        private List<LinkableChip> FindConnectedChips(LinkableChip startChip, GridSystem<Chip> gridSystem)
        {
            var visited = new HashSet<Vector2Int>();
            var toVisit = new Stack<Vector2Int>();
            var connectedChips = new List<LinkableChip>();

            var targetColor = startChip.ColorType;

            toVisit.Push(startChip.Position);

            while (toVisit.Count > 0)
            {
                var currentPos = toVisit.Pop();
                if (!gridSystem.CheckBounds(currentPos)) continue;

                if (visited.Contains(currentPos)) continue;

                var chip = gridSystem.GetItemAt(currentPos) as LinkableChip;
                if (chip == null || chip.ColorType != targetColor) continue;

                visited.Add(currentPos);
                connectedChips.Add(chip);

                foreach (var dir in _directions)
                    toVisit.Push(currentPos + dir);
            }

            return connectedChips;
        }
    }
}