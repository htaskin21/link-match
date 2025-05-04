using System.Collections.Generic;
using Chips;
using Cores;
using UnityEngine;

namespace Logic
{
    public class BoardShuffler
    {
        private readonly List<Chip> _chipBuffer = new();
        private readonly List<(Vector3 worldPos, Vector2Int gridPos)> _positionBuffer = new();

        public Dictionary<Vector2Int, List<LinkableChip>> ShuffleUntilMatch(
            GridSystem<Chip> grid,
            IChipMatcher matcher,
            Transform gridTransform,
            int maxAttempts = 20)
        {
            Collect(grid);

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                ShufflePositions();
                ClearGrid(grid);
                ApplyShuffled(grid, gridTransform);

                var result = matcher.GenerateMatchCache(grid);
                if (result.Count > 0)
                    return result;
            }

            Debug.LogWarning("GridShuffler: Max denemede eşleşme bulunamadı.");
            return new();
        }

        private void Collect(GridSystem<Chip> grid)
        {
            _chipBuffer.Clear();
            _positionBuffer.Clear();

            for (int y = 0; y < grid.GridSize.y; y++)
            {
                for (int x = 0; x < grid.GridSize.x; x++)
                {
                    Vector2Int pos = new(x, y);
                    if (grid.GetItemAt(pos) is LinkableChip chip)
                    {
                        _chipBuffer.Add(chip);
                        _positionBuffer.Add((chip.transform.position, pos));
                    }
                }
            }
        }

        private void ShufflePositions()
        {
            for (int i = _positionBuffer.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (_positionBuffer[i], _positionBuffer[j]) = (_positionBuffer[j], _positionBuffer[i]);
            }
        }

        private void ClearGrid(GridSystem<Chip> grid)
        {
            foreach (var chip in _chipBuffer)
            {
                grid.RemoveItemAt(chip.Position.x, chip.Position.y);
            }
        }

        private void ApplyShuffled(GridSystem<Chip> grid, Transform gridTransform)
        {
            for (int i = 0; i < _chipBuffer.Count; i++)
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