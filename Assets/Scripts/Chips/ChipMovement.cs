using DG.Tweening;
using UnityEngine;

namespace Chips
{
    public class ChipMovement
    {
        private readonly float _speed;
        private readonly Ease _easeType;
        private Tween _activeTween;

        public ChipMovement(float speed = 8f, Ease ease = Ease.Linear)
        {
            _speed = speed;
            _easeType = ease;
        }

        public Tween Move(GameObject chip, Vector3 targetPos)
        {
            _activeTween?.Kill();
            
            var duration = FindDuration(chip, targetPos);
            _activeTween = chip.transform.DOMove(targetPos, duration)
                .SetEase(_easeType);

            return _activeTween;
        }

        private float FindDuration(GameObject chip, Vector3 targetPos)
        {
            var distance = Vector3.Distance(chip.transform.position, targetPos);
            var duration = distance / _speed;
            return duration;
        }
    }
}