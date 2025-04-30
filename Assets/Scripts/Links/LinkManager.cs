using System.Collections.Generic;
using Chips;
using Cores;
using UnityEngine;

namespace Links
{
    public class LinkManager : MonoBehaviour
    {
        public static LinkManager Instance { get; private set; }

        private readonly List<LinkableChip> _link = new();
        private int _minLinkCount = 3;

        private GridSystem<Chip> _gridSystem;
        
        private void Awake()
        {
            Instance = this;
        }

        public void Init(GridSystem<Chip> gridSystem)
        {
            _gridSystem = gridSystem;
        }

        public void StartLink() => _link.Clear();

        public void StartLinkAt(Vector2 worldPos)
        {
            _link.Clear();

            Vector2Int gridPos = Vector2Int.RoundToInt(worldPos);
            if (!_gridSystem.CheckBounds(gridPos)) return;

            if (_gridSystem.GetItemAt(gridPos) is LinkableChip chip)
            {
                TryAdd(chip);
            }
        }
        
        public void TryAdd(LinkableChip chip)
        {
            if (_link.Contains(chip)) return;

            if (_link.Count == 0 || IsValidNext(chip))
            {
                _link.Add(chip);
                chip.Highlight(true);
            }
        }

        public void EndLink()
        {
            if (_link.Count >= _minLinkCount)
            {
                foreach (var chip in _link)
                {
                    chip.Destroy();
                }
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
