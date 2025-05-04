namespace Chips
{
    // Extends Chip to support linking logic
    public class LinkableChip : Chip
    {
        public IconController IconController { get; private set; }

        public ChipMovement ChipMovement { get; private set; }

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

        public override void Disable()
        {
            IconController.PlayParticle(transform);
        }
    }
}