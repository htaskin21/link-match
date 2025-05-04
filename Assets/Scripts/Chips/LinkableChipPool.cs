using Cores;
using UnityEngine;
using Random = System.Random;

namespace Chips
{
    // Provides LinkableChips with assigned icons.
    public class LinkableChipPool : ObjectPool<LinkableChip>
    {
        [SerializeField]
        private LinkableChipIconSO[] _linkableChipIconSos;

        private int _numberOfColors;
        private Random _random;

        public void Init(int numberOfColors)
        {
            _numberOfColors = numberOfColors;
            _random = new Random();
        }

        public LinkableChip GetRandomChip()
        {
            var chip = GetObject();
            var randomIconIndex = _random.Next(0, _numberOfColors);
            chip.SetType(_linkableChipIconSos[randomIconIndex]);
            return chip;
        }
    }
}