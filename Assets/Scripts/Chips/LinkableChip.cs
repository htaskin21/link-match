using System;
using Links;
using UnityEngine;
using UnityEngine.EventSystems;

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

        public void OnPointerClick(PointerEventData eventData)
        {
            //ChipClicked?.Invoke(this);
        }
        
        private void OnMouseEnter()
        {
            if (!LinkInputController.IsLinking) return;
            LinkManager.Instance.TryAdd(this);
        }

        public void Destroy()
        {
            // Grid’den de silinmeli, geçici olarak sadece görünmez yapalım
            gameObject.SetActive(false);
        }

        public void Highlight(bool on)
        {
            transform.localScale = on ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}