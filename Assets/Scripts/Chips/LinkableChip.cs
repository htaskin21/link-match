using Links;
using UnityEngine;

namespace Chips
{
    public class LinkableChip : Chip
    {
        private IconController _iconController;

        public ChipMovement ChipMovement { get; private set; }

        public void Awake()
        {
            _iconController = new IconController(_spriteRenderer);
            ChipMovement = new ChipMovement();
        }

        public void SetType(LinkableChipIconSO iconSO)
        {
            ColorType = iconSO.ColorType;
            _iconController.SetIconSO(iconSO);
        }

        private void OnMouseEnter()
        {
            if (!LinkInputController.Instance.IsLinking) return;
            LinkInputController.Instance.LinkManager.HandleChipEntered(this);
        }

        public void Highlight(bool on)
        {
            transform.localScale = on ? Vector3.one * 1.2f : Vector3.one;
        }

        public override void Destroy()
        {
            _iconController.PlayParticle(transform);
        }
    }
}