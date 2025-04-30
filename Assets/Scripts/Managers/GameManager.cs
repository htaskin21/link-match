using Chips;
using Logic;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private LevelManager _levelManager;

        [SerializeField]
        private CameraController _cameraController;

        [SerializeField]
        private GridManager _gridManager;

        [SerializeField]
        private LinkableChipPool _linkableChipPool;

        private void Start()
        {
            _cameraController.Setup(_levelManager.RowSize, _levelManager.ColumnSize);

            _linkableChipPool.Init(_levelManager.NumberOfColors);
            var poolSize = _levelManager.ColumnSize * _levelManager.RowSize * 2;
            _linkableChipPool.CreatePool(poolSize);

            var chipMatcher = new ChipMatcher();
            var gravityController = new GravityController();
            var boardRefiller = new BoardRefiller();
            _gridManager.Init(_levelManager.ColumnSize,
                _levelManager.RowSize, _linkableChipPool, chipMatcher,
                gravityController, boardRefiller);
            _gridManager.PopulateGrid();
        }
    }
}