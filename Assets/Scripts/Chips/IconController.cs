using Particles;
using UnityEngine;

namespace Chips
{
    public class IconController
    {
        private readonly SpriteRenderer _spriteRenderer;
        private LinkableChipIconSO _chipIconSo;

        public IconController(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
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
    }
}