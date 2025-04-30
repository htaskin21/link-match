using DG.Tweening;
using UnityEngine;

namespace Chips
{
    public class ChipMovement
    {
        private readonly float _speed;
        private readonly Ease _easeType;
        private Tween _activeTween;

        public ChipMovement(float speed = 0.2f, Ease ease = Ease.Linear)
        {
            _speed = speed;
            _easeType = ease;
        }

        public Tween Move(GameObject chip, Vector3 targetPos)
        {
            _activeTween?.Kill();

            _activeTween = chip.transform.DOMove(targetPos, _speed)
                .SetEase(_easeType);

            return _activeTween;
        }
    }
}