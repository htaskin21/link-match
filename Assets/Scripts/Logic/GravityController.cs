using System.Collections.Generic;
using Chips;
using Cores;
using DG.Tweening;
using UnityEngine;

namespace Logic
{
    // Applies gravity by dropping chips into empty spaces, returning a Sequence of falling tweens.
    public class GravityController
    {
        private Sequence _fallSequence;
        private readonly List<Tween> _individualTweens = new();

        /// <summary>
        /// Moves chips down into empty cells with animations and returns a Sequence tracking them.
        /// </summary>
        public Sequence ApplyGravity(GridSystem<Chip> grid, Vector3 boardOrigin)
        {
            KillActiveTweens();
            _fallSequence = DOTween.Sequence();

            for (var x = 0; x < grid.GridSize.x; x++)
            {
                var emptyCount = 0;

                for (var y = 0; y < grid.GridSize.y; y++)
                {
                    var currentPos = new Vector2Int(x, y);

                    if (grid.IsEmpty(currentPos))
                    {
                        emptyCount++;
                    }
                    else if (emptyCount > 0)
                    {
                        var targetPos = new Vector2Int(x, y - emptyCount);

                        if (grid.GetItemAt(currentPos) is not LinkableChip chip)
                            continue;

                        grid.MoveItemTo(currentPos, targetPos);
                        chip.SetSortOrder(targetPos.y);
                        var worldTarget = boardOrigin + new Vector3(targetPos.x, targetPos.y);
                        var moveTween = chip.ChipMovement.Move(chip.gameObject, worldTarget)
                            .OnComplete(() => chip.SetPosition(boardOrigin, targetPos.x, targetPos.y));

                        _individualTweens.Add(moveTween);
                        _fallSequence.Join(moveTween);
                    }
                }
            }

            return _fallSequence;
        }

        public void KillActiveTweens()
        {
            _fallSequence?.Kill();

            foreach (var tween in _individualTweens)
            {
                tween?.Kill();
            }

            _individualTweens.Clear();
        }
    }
}