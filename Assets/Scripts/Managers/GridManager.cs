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
        private BoardShuffler _boardShuffler;
        private Dictionary<Vector2Int, List<LinkableChip>> _matchCache;

        public void Init(int columnSize, int rowSize, LinkableChipPool linkableChipPool, IChipMatcher chipMatcher,
            GravityController gravityController, BoardRefiller boardRefiller, BoardShuffler boardShuffler)
        {
            GridSize = new Vector2Int(columnSize, rowSize);
            CreateGrid();
            _linkableChipPool = linkableChipPool;
            _chipMatcher = chipMatcher;
            _gravityController = gravityController;
            _boardRefiller = boardRefiller;
            _boardShuffler = boardShuffler;
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
                    PutItemAt(chip, x, y);
                }
            }

            UpdateMatchCache();
        }

        public void CheckMatch(List<LinkableChip> link)
        {
            foreach (var chip in link)
            {
               chip.Destroy();
               RemoveItemAt(chip.Position);
               _linkableChipPool.ReturnToPool(chip);
            }

            _gravityController.ApplyGravity(this, transform.position)
                .OnComplete(RefillAfterGravity);
        }

        private void RemoveChip(LinkableChip chip)
        {
            RemoveItemAt(chip.Position);
            _linkableChipPool.ReturnToPool(chip);
        }

        private void RefillAfterGravity()
        {
            _boardRefiller.SpawnNewChips(this, _linkableChipPool, transform.position)
                .OnComplete(UpdateMatchCache);
        }

        private void UpdateMatchCache()
        {
            _matchCache = _chipMatcher.GenerateMatchCache(this);

            if (_matchCache.Count == 0)
            {
                Shuffle();
            }
        }

        public void Shuffle()
        {
            _matchCache = _boardShuffler.ShuffleUntilMatch(this, _chipMatcher, transform);
        }

        private void OnDestroy()
        {
            _gravityController.KillActiveTweens();
            _boardRefiller.KillRefillSequence();
        }
    }
}