using DG.Tweening;
using UnityEngine;

namespace Chips
{
    // Provides smooth, speed-based movement for Chip GameObjects using DOTween.
    public class ChipMovement
    {
        private Tween _activeTween;
        private readonly float _speed;
        private readonly Ease _easeType;

        public ChipMovement(float speed = 10f, Ease ease = Ease.Linear)
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

        /// <summary>
        /// Calculates tween duration based on distance and configured speed.
        /// </summary>
        private float FindDuration(GameObject chip, Vector3 targetPos)
        {
            var distance = Vector3.Distance(chip.transform.position, targetPos);
            var duration = distance / _speed;
            return duration;
        }
    }
}