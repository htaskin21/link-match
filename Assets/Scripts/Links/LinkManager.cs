using System.Collections.Generic;
using Chips;
using Managers;
using UnityEngine;

namespace Links
{
    public class LinkManager
    {
        private readonly List<LinkableChip> _link;
        private const int MinLinkCount = 3;

        private readonly GridManager _gridManager;

        public LinkManager(GridManager gridManager)
        {
            _link = new List<LinkableChip>();
            _gridManager = gridManager;
        }

        public void StartLinkAt(Vector2 worldPos)
        {
            _link.Clear();

            Vector2Int gridPos = Vector2Int.RoundToInt(worldPos);
            if (!_gridManager.CheckBounds(gridPos)) return;

            if (_gridManager.GetItemAt(gridPos) is LinkableChip chip)
            {
                HandleChipEntered(chip);
            }
        }

        public void HandleChipEntered(LinkableChip chip)
        {
            if (TryHandleBackStep(chip)) return;
            TryAdd(chip);
        }

        private bool TryHandleBackStep(LinkableChip chip)
        {
            if (_link.Count > 1 && chip == _link[^2])
            {
                var removed = _link[^1];
                _link.RemoveAt(_link.Count - 1);
                removed.Highlight(false);
                return true;
            }
            return false;
        }

        private void TryAdd(LinkableChip chip)
        {
            if (_link.Contains(chip))
            {
                if (_link[^1] != chip && (_link.Count < 2 || _link[^2] != chip))
                    return;
            }

            if (_link.Count == 0 || IsValidNext(chip))
            {
                _link.Add(chip);
                chip.Highlight(true);
            }
        }

        public void EndLink()
        {
            if (_link.Count >= MinLinkCount)
            {
                _gridManager.CheckMatch(_link);
            }

            foreach (var chip in _link)
            {
                chip.Highlight(false);
            }

            _link.Clear();
        }

        private bool IsValidNext(LinkableChip chip)
        {
            var last = _link[^1];
            bool isAdjacent = Mathf.Abs(chip.Position.x - last.Position.x) + Mathf.Abs(chip.Position.y - last.Position.y) == 1;
            return isAdjacent && chip.ColorType == last.ColorType;
        }
    }
}