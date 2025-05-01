using Chips;
using Cores;
using DG.Tweening;
using UnityEngine;

namespace Logic
{
    public class BoardRefiller
    {
        private Sequence _refillSequence;

        public Sequence SpawnNewChips(GridSystem<Chip> grid, LinkableChipPool pool, Vector3 boardOrigin)
        {
            _refillSequence?.Kill();
            _refillSequence = DOTween.Sequence();

            for (var x = 0; x < grid.GridSize.x; x++)
            {
                var spawnOffset = 0;

                for (var y = grid.GridSize.y - 1; y >= 0; y--)
                {
                    Vector2Int pos = new(x, y);
                    if (!grid.IsEmpty(pos))
                        continue;

                    var spawnY = grid.GridSize.y + spawnOffset;
                    var chip = pool.GetRandomChip();
                    chip.SetPosition(boardOrigin, x, spawnY);
                    chip.gameObject.SetActive(true);
                    grid.PutItemAt(chip, pos);

                    var targetWorldPos = boardOrigin + new Vector3(x, y);
                    _refillSequence.Join(chip.ChipMovement.Move(chip.gameObject, targetWorldPos)
                        .OnComplete(() => chip.SetPosition(boardOrigin, pos.x, pos.y)));

                    spawnOffset++;
                }
            }

            return _refillSequence;
        }

        public void KillRefillSequence()
        {
            _refillSequence?.Kill();
        }
    }
}