using System.Collections.Generic;
using Chips;
using Cores;
using Matchers;
using UnityEngine;

namespace GridFlow
{
    // Randomly shuffles existing chips on the grid until at least one valid match is found.
    public class BoardShuffler
    {
        private readonly List<Chip> _chipBuffer = new();
        private readonly List<(Vector3 worldPos, Vector2Int gridPos)> _positionBuffer = new();

        /// <summary>
        /// Attempts up to maxAttempts to shuffle grid positions until matcher finds a match.
        /// </summary>
        public Dictionary<Vector2Int, List<LinkableChip>> ShuffleUntilMatch(
            GridSystem<Chip> grid,
            IChipMatcher matcher,
            Transform gridTransform,
            int maxAttempts = 50)
        {
            Collect(grid);

            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                ShufflePositions();
                ClearGrid(grid);
                ApplyShuffled(grid, gridTransform);

                var result = matcher.GenerateMatchCache(grid);
                if (result.Count > 0)
                    return result;
            }

            return new Dictionary<Vector2Int, List<LinkableChip>>();
        }

        /// <summary>
        /// Collects all existing LinkableChip instances and their positions.
        /// </summary>
        private void Collect(GridSystem<Chip> grid)
        {
            _chipBuffer.Clear();
            _positionBuffer.Clear();

            foreach (var pos in grid.AllPositions())
            {
                if (grid.GetItemAt(pos) is LinkableChip chip)
                {
                    _chipBuffer.Add(chip);
                    _positionBuffer.Add((chip.transform.position, pos));
                }
            }
        }

        // Shuffles the saved position buffer in-place using Fisher–Yates algorithm.
        private void ShufflePositions()
        {
            for (var i = _positionBuffer.Count - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (_positionBuffer[i], _positionBuffer[j]) = (_positionBuffer[j], _positionBuffer[i]);
            }
        }

        /// <summary>
        /// Clears all buffered chips from the grid data store.
        /// </summary>
        private void ClearGrid(GridSystem<Chip> grid)
        {
            foreach (var chip in _chipBuffer)
            {
                grid.RemoveItemAt(chip.Position.x, chip.Position.y);
            }
        }

        /// <summary>
        /// Applies shuffled positions back to grid and world transforms.
        /// </summary>
        private void ApplyShuffled(GridSystem<Chip> grid, Transform gridTransform)
        {
            for (var i = 0; i < _chipBuffer.Count; i++)
            {
                var chip = _chipBuffer[i];
                var (world, gridPos) = _positionBuffer[i];

                chip.SetPosition(gridTransform.position, gridPos.x, gridPos.y);
                chip.transform.position = world;
                chip.SetSortOrder(gridPos.y);
                grid.PutItemAt(chip, new Vector2Int(gridPos.x, gridPos.y));
            }
        }
    }
}