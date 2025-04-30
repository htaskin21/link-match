using System;
using Links;
using UnityEngine;

namespace Chips
{
    public class LinkableChip : Chip
    {
        public IconController IconController { get; private set; }

        public ChipMovement ChipMovement { get; private set; }

        public event Action<Chip> ChipClicked;

        public void Awake()
        {
            IconController = new IconController(_spriteRenderer);
            ChipMovement = new ChipMovement();
        }

        public void SetType(LinkableChipIconSO iconSO)
        {
            ColorType = iconSO.ColorType;
            IconController.SetIconSO(iconSO);
        }
        
        private void OnMouseEnter()
        {
            if (!LinkInputController.Instance.IsLinking) return;
            LinkInputController.Instance.LinkManager.HandleChipEntered(this);
        }

        public void Destroy()
        {
            ChipClicked?.Invoke(this);
        }

        public void Highlight(bool on)
        {
            transform.localScale = on ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}