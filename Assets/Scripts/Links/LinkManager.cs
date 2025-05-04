using System.Collections.Generic;
using Chips;
using Logic;
using Managers;
using UnityEngine;

namespace Links
{
    // Selection order, back-stepping, match completion, and rule resolution.
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

        /// <summary>
        /// Begins link at world position by resetting previous link and handling first chip.
        /// </summary>
        public void StartLinkAt(Vector2 worldPos)
        {
            _link.Clear();
            _linkVisual.ResetLine();

            var gridPos = Vector2Int.RoundToInt(worldPos);
            if (!_gridManager.CheckBounds(gridPos)) return;

            if (_gridManager.GetItemAt(gridPos) is LinkableChip chip)
            {
                HandleChipEntered(chip);
            }
        }

        /// <summary>
        /// Handles a chip being entered into the link chain, including back-step logic.
        /// </summary>
        private void HandleChipEntered(LinkableChip chip)
        {
            if (TryHandleBackStep(chip)) return;
            TryAdd(chip);
        }

        /// <summary>
        /// Removes the last chip if user drags back to previous chip.
        /// </summary>
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

        /// <summary>
        /// Adds a valid next chip to the link and updates visuals.
        /// </summary>
        private void TryAdd(LinkableChip chip)
        {
            // Prevent duplicates except for back-step
            if (_link.Contains(chip))
            {
                if (_link[^1] != chip && (_link.Count < 2 || _link[^2] != chip))
                    return;
            }

            // Check adjacency and color match
            if (_link.Count == 0 || IsValidNext(chip))
            {
                _link.Add(chip);
                chip.IconController.Highlight(true);
                _linkVisual.AddPoint(chip.transform.position);
            }
        }

        /// <summary>
        /// Ends the link: triggers match removal and rule resolution if valid.
        /// </summary>
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

        /// <summary>
        /// Checks if the given chip is adjacent and same color as last.
        /// </summary>
        private bool IsValidNext(LinkableChip chip)
        {
            var last = _link[^1];
            var isAdjacent = Mathf.Abs(chip.Position.x - last.Position.x) +
                Mathf.Abs(chip.Position.y - last.Position.y) == 1;
            return isAdjacent && chip.ColorType == last.ColorType;
        }
        
        /// <summary>
        /// Attempts to continue link based on world position for touch drag.
        /// </summary>
        public void TryContinueAt(Vector2 worldPos)
        {
            var gridPos = Vector2Int.RoundToInt(worldPos);
            if (!_gridManager.CheckBounds(gridPos)) return;

            if (_gridManager.GetItemAt(gridPos) is LinkableChip chip)
            {
                HandleChipEntered(chip);
            }
        }
    }
}