using UnityEngine;

namespace Chips
{
    [CreateAssetMenu(fileName = "IconSO", menuName = "ScriptableObjects/LinkableChipIconSO", order = 1)]
    public class LinkableChipIconSO : ScriptableObject
    {
        [SerializeField]
        private ChipColorType _colorType;

        public ChipColorType ColorType => _colorType;

        [SerializeField]
        private Sprite _icon;

        public Sprite Icon => _icon;
    }
}