using System;
using UnityEngine.EventSystems;

namespace Chips
{
    public class LinkableChip : Chip, IPointerClickHandler
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
            ChipClicked?.Invoke(this);
        }
    }
}