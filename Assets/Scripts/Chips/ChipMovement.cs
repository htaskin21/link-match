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
            var speed = FindSpeed(chip, targetPos);
            _activeTween = chip.transform.DOMove(targetPos, _speed)
                .SetEase(_easeType);

            return _activeTween;
        }

        private float FindSpeed(GameObject chip, Vector3 targetPos)
        {
            var startPos = chip.transform.position;
            var distance = Vector3.Distance(startPos, targetPos);
            var duration = distance / _speed;
            return duration;
        }
    }
}