using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Board Size")]
        [SerializeField]
        private int _rowSize;

        public int RowSize => _rowSize;

        [SerializeField]
        private int _columnSize;

        public int ColumnSize => _columnSize;

        [Header("Linkable Chip Color Amount")]
        [SerializeField]
        private int _numberOfColors;

        public int NumberOfColors => _numberOfColors;
    }
}