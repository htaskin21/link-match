using UnityEngine;
using UnityEngine.Serialization;

namespace Logic
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelDataSO", order = 2)]
    public class LevelDataSO : ScriptableObject
    {
        [Header("Board Size")]
        [SerializeField]
        private int _rowSize;

        public int RowSize => _rowSize;

        [SerializeField]
        private int _columnSize;

        public int ColumnSize => _columnSize;
        
        [Header("Number Of Different Chips")]
        [SerializeField]
        private int _numberOfDifferentChips;

        public int NumberOfDifferentChips => _numberOfDifferentChips;

        [Header("Win Conditions")]
        [SerializeField]
        private int _moveAmount;

        public int MoveAmount => _moveAmount;

        [SerializeField]
        private int _reqWinScore;

        public int ReqWinScore => _reqWinScore;
    }
}
