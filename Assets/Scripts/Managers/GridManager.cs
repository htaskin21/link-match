using System.Collections.Generic;
using Chips;
using Cores;
using DG.Tweening;
using Extensions;
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
                    PutItemAt(chip, x, y);
                }
            }

            UpdateMatchCache();
        }

        public void CheckMatch(List<LinkableChip> link)
        {
            foreach (var chip in link)
            {
                RemoveItemAt(chip.Position);
                _linkableChipPool.ReturnToPool(chip);
            }

            _gravityController.ApplyGravity(this, transform.position)
                .OnComplete(RefillAfterGravity);
        }

        private void RefillAfterGravity()
        {
            _boardRefiller.SpawnNewChips(this, _linkableChipPool, transform.position)
                .OnComplete(UpdateMatchCache);
        }

        private void UpdateMatchCache()
        {
            _matchCache = _chipMatcher.GenerateMatchCache(this);

            if (_matchCache.Count < 1)
            {
                do
                {
                    var chipPositions = new List<ChipPositionData>();
                    var chips = new List<Chip>();
                    for (var y = 0; y < GridSize.y; y++)
                    {
                        for (var x = 0; x < GridSize.x; x++)
                        {
                            var chip = GetItemAt(x, y);
                            chips.Add(chip);
                            chipPositions.Add(new ChipPositionData(chip.transform.position, chip.Position));
                        }
                    }

                    chipPositions.Shuffle();

                    for (var y = 0; y < GridSize.y; y++)
                    {
                        for (var x = 0; x < GridSize.x; x++)
                        {
                            RemoveItemAt(x, y);
                        }
                    }

                    for (int i = 0; i < chips.Count; i++)
                    {
                        var pos = chipPositions[i];
                        var chip = chips[i];

                        chips[i].SetPosition(transform.position, pos.Position.x, pos.Position.y);
                       //chip.transform.position = pos.WorldPosition;
                        PutItemAt(chips[i], pos.Position.x, pos.Position.y);
                    }
                    _matchCache = _chipMatcher.GenerateMatchCache(this);
                } while (_matchCache.Count < 1);
            }
        }


        private void OnDestroy()
        {
            _gravityController.KillActiveTweens();
            _boardRefiller.KillRefillSequence();
        }
    }

    public class ChipPositionData
    {
        public readonly Vector3 WorldPosition;
        public readonly Vector2Int Position;

        public ChipPositionData(Vector3 worldPosition, Vector2Int position)
        {
            WorldPosition = worldPosition;
            Position = position;
        }
    }
}