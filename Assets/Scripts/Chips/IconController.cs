using Particles;
using UnityEngine;

namespace Chips
{
    // Manages the visual processing of Chips
    public class IconController
    {
        private LinkableChipIconSO _chipIconSo;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Vector3 _localScale;
        private const float HighlightMultiplier = 1.2f;

        public IconController(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
            _localScale = _spriteRenderer.transform.localScale;
        }

        public void SetIconSO(LinkableChipIconSO chipIconSo)
        {
            _chipIconSo = chipIconSo;
            _spriteRenderer.sprite = _chipIconSo.Icon;
        }

        public void PlayParticle(Transform transform)
        {
            var particle = ParticleManager.Instance.GetChipParticle();
            particle.PlayAndRelease(ParticleManager.Instance.ChipParticlePool, _chipIconSo.Color, transform.position);
        }

        public void Highlight(bool on)
        {
            _spriteRenderer.transform.localScale = on ? _localScale * HighlightMultiplier : _localScale;
        }
    }
}