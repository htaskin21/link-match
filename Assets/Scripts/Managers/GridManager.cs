using System.Collections.Generic;
using Chips;
using Cores;
using DG.Tweening;
using Logic;
using UnityEngine;

namespace Managers
{
    public class GridManager : GridSystem<Chip>
    {
        [SerializeField]
        private Vector2 _offScreenOffSet;

        private LinkableChipPool _linkableChipPool;
        private IChipMatcher _chipMatcher;
        private GravityController _gravityController;
        private BoardRefiller _boardRefiller;
        private Dictionary<Vector2Int, List<LinkableChip>> _matchCache;

        public void Init(int columnSize, int rowSize, LinkableChipPool linkableChipPool, IChipMatcher chipMatcher,
            GravityController gravityController, BoardRefiller boardRefiller)
        {
            GridSize = new Vector2Int(columnSize, rowSize);
            CreateGrid();
            _linkableChipPool = linkableChipPool;
            _chipMatcher = chipMatcher;
            _gravityController = gravityController;
            _boardRefiller = boardRefiller;
        }

        public void PopulateGrid()
        {
            for (var y = 0; y < GridSize.y; y++)
            {
                for (var x = 0; x < GridSize.x; x++)
                {
                    if (!IsEmpty(x, y)) continue;
                    var chip = _linkableChipPool.GetRandomChip();
                    chip.SetPosition(transform.position, x, y);
                    chip.gameObject.SetActive(true);
                    chip.ChipClicked += CheckMatch;
                    PutItemAt(chip, x, y);
                }
            }

            UpdateMatchCache();
        }

        private void CheckMatch(Chip clickedChip)
        {
            var group = GetMatchedGroupIfAny(clickedChip.Position);
            if (group.Count >= 2)
            {
                foreach (var chip in group)
                {
                    chip.ChipClicked -= CheckMatch;
                    RemoveItemAt(chip.Position);
                    _linkableChipPool.ReturnToPool(chip);
                }

                _gravityController.ApplyGravity(this, transform.position)
                    .OnComplete(RefillAfterGravity);
            }
        }

        public void RemoveChipsFromBoard(List<LinkableChip> linkedChips)
        {
            foreach (var chip in linkedChips)
            {
                chip.ChipClicked -= CheckMatch;
                RemoveItemAt(chip.Position);
                _linkableChipPool.ReturnToPool(chip);
            }

            _gravityController.ApplyGravity(this, transform.position)
                .OnComplete(RefillAfterGravity);
        }

        private void RefillAfterGravity()
        {
            _boardRefiller.SpawnNewChips(this, _linkableChipPool, transform.position, CheckMatch)
                .OnComplete(UpdateMatchCache);
        }

        private void UpdateMatchCache()
        {
            _matchCache = _chipMatcher.GenerateMatchCache(this);
        }

        private List<LinkableChip> GetMatchedGroupIfAny(Vector2Int pos)
        {
            return _matchCache.TryGetValue(pos, out var group) ? group : new List<LinkableChip>();
        }

        private void OnDestroy()
        {
            _gravityController.KillActiveTweens();
            _boardRefiller.KillRefillSequence();
        }
    }
}