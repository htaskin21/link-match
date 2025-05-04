using System.Collections.Generic;
using Chips;
using Logic;
using Managers;
using UnityEngine;

namespace Links
{
    public class LinkManager
    {
        private readonly GridManager _gridManager;
        private readonly LinkVisualController _linkVisual;
        private readonly IGameRuler _gameRuleManager;
       
        private readonly List<LinkableChip> _link;
        private const int MinLinkCount = 3;

        public LinkManager(GridManager gridManager, LinkVisualController linkVisual, IGameRuler gameRuleManager)
        {
            _gridManager = gridManager;
            _linkVisual = linkVisual;
            _gameRuleManager = gameRuleManager;
            _link = new List<LinkableChip>();
        }

        public void StartLinkAt(Vector2 worldPos)
        {
            _link.Clear();
            _linkVisual.ResetLine();

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
                removed.IconController.Highlight(false);
                _linkVisual.RemoveLastPoint();
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
                chip.IconController.Highlight(true);
                _linkVisual.AddPoint(chip.transform.position);
            }
        }

        public void EndLink()
        {
            if (_link.Count >= MinLinkCount)
            {
                _gridManager.CheckMatchAndRefill(_link);
                _gameRuleManager.ResolveLink(_link);
            }

            foreach (var chip in _link)
            {
                chip.IconController.Highlight(false);
            }

            _link.Clear();
            _linkVisual.ResetLine();
        }

        private bool IsValidNext(LinkableChip chip)
        {
            var last = _link[^1];
            bool isAdjacent = Mathf.Abs(chip.Position.x - last.Position.x) +
                Mathf.Abs(chip.Position.y - last.Position.y) == 1;
            return isAdjacent && chip.ColorType == last.ColorType;
        }
        
        public void TryContinueAt(Vector2 worldPos)
        {
            Vector2Int gridPos = Vector2Int.RoundToInt(worldPos);
            if (!_gridManager.CheckBounds(gridPos)) return;

            if (_gridManager.GetItemAt(gridPos) is LinkableChip chip)
            {
                HandleChipEntered(chip);
            }
        }
    }
}