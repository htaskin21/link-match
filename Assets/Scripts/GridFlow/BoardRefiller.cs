using Chips;
using Cores;
using DG.Tweening;
using UnityEngine;

namespace GridFlow
{
    // Fills empty grid cells by spawning new chips and animating their fall; returns a Sequence that completes when all chip movements finish.
    public class BoardRefiller
    {
        private Sequence _refillSequence;

        // Animates spawning chips into empty cells and returns a Sequence tracking all moves.
        public Sequence SpawnNewChips(GridSystem<Chip> grid, LinkableChipPool pool, Vector3 boardOrigin)
        {
            _refillSequence?.Kill();
            _refillSequence = DOTween.Sequence();

            for (var x = 0; x < grid.GridSize.x; x++)
            {
                var spawnOffset = 0;

                for (var y = 0; y < grid.GridSize.y; y++)
                {
                    Vector2Int pos = new(x, y);
                    if (!grid.IsEmpty(pos))
                        continue;

                    var spawnY = grid.GridSize.y + spawnOffset;
                    var chip = pool.GetRandomChip();
                    chip.SetPosition(boardOrigin, x, spawnY);
                    chip.gameObject.SetActive(true);
                    grid.PutItemAt(chip, pos);

                    var targetWorldPos = boardOrigin + new Vector3(x, y, 0);
                    chip.SetSortOrder(y);
                    _refillSequence.Join(
                        chip.ChipMovement.Move(chip.gameObject, targetWorldPos)
                            .OnComplete(() => chip.SetPosition(boardOrigin, pos.x, pos.y))
                    );

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