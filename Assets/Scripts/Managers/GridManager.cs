using System.Collections.Generic;
using Chips;
using Cores;
using DG.Tweening;
using Logic;
using UnityEngine;

namespace Managers
{
    // Manages grid item lifecycle: population, matching, gravity, refill, and shuffle.
    public class GridManager : GridSystem<Chip>
    {
        [SerializeField]
        private Vector2 _offScreenOffset;
        
        private LinkableChipPool _chipPool;
        private IChipMatcher _matcher;
        private GravityController _gravity;
        private BoardRefiller _refiller;
        private BoardShuffler _shuffler;
        private Dictionary<Vector2Int, List<LinkableChip>> _matchCache;

        /// <summary>
        /// Indicates when the board has settled after refill
        /// </summary>
        public bool AreAllChipsPlaced { get; private set; } = false;

        public void Init(
            int columns,
            int rows,
            LinkableChipPool chipPool,
            IChipMatcher matcher,
            GravityController gravity,
            BoardRefiller refiller,
            BoardShuffler shuffler)
        {
            _chipPool = chipPool;
            _matcher = matcher;
            _gravity = gravity;
            _refiller = refiller;
            _shuffler = shuffler;

            ResizeGrid(columns, rows);
        }

        /// <summary>
        /// Fills empty cells and ensures at least one match exists.
        /// </summary>
        public void PopulateGrid()
        {
            FillEmptyCells();
            EnsureMatch();
        }

        /// <summary>
        /// Instantiates and places a random chip in each empty grid cell.
        /// </summary>
        private void FillEmptyCells()
        {
            foreach (var pos in AllPositions())
            {
                if (IsEmpty(pos))
                {
                    var chip = _chipPool.GetRandomChip();
                    chip.SetPosition(transform.position, pos.x, pos.y);
                    chip.SetSortOrder(pos.y);
                    chip.gameObject.SetActive(true);
                    PutItemAt(chip, pos);
                }
            }
        }

        /// <summary>
        /// Checks current matches and shuffles board if none found.
        /// </summary>
        private void EnsureMatch()
        {
            _matchCache = _matcher.GenerateMatchCache(this);
            if (_matchCache.Count == 0)
                ShuffleUntilMatch();
        }

        public void ShuffleUntilMatch(int maxAttempts = 50)
        {
            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                _matchCache = _shuffler.ShuffleUntilMatch(this, _matcher, transform);
                if (_matchCache.Count > 0)
                    return;
            }

            Debug.LogWarning("GridManager: Shuffle limit reached without finding a match.");
        }

        public void CheckMatchAndRefill(List<LinkableChip> link)
        {
            AreAllChipsPlaced = false;
            RemoveLinkedChips(link);
            ApplyGravityAndRefill();
        }

        private void RemoveLinkedChips(IEnumerable<LinkableChip> link)
        {
            foreach (var chip in link)
            {
                chip.Disable();
                RemoveItemAt(chip.Position);
                _chipPool.ReturnToPool(chip);
            }
        }

        private void ApplyGravityAndRefill()
        {
            _gravity.ApplyGravity(this, transform.position)
                .OnComplete(RefillAfterGravity);
        }

        private void RefillAfterGravity()
        {
            _refiller.SpawnNewChips(this, _chipPool, transform.position)
                .OnComplete(()=>
                {
                    AreAllChipsPlaced = true;
                    EnsureMatch();
                });
        }

        public void ClearGrid()
        {
            foreach (var pos in AllPositions())
            {
                if (!IsEmpty(pos) && GetItemAt(pos) is LinkableChip chip)
                {
                    RemoveItemAt(pos);
                    _chipPool.ReturnToPool(chip);
                }
            }
        }

        public void ResizeGrid(int columns, int rows)
        {
            GridSize = new Vector2Int(columns, rows);
            CreateGrid();
        }

        private void OnDestroy()
        {
            _gravity.KillActiveTweens();
            _refiller.KillRefillSequence();
        }
    }
}