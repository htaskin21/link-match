using Chips;
using Links;
using Logic;
using Tiles;
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
        private LinkInputController _linkInputController;

        [SerializeField]
        private LinkVisualController _linkVisualController;

        [SerializeField]
        private TileManager _tileManager;

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
            var boardShuffler = new BoardShuffler();
            _gridManager.Init(_levelManager.ColumnSize,
                _levelManager.RowSize, _linkableChipPool, chipMatcher,
                gravityController, boardRefiller, boardShuffler);
            _tileManager.CreateTiles(_levelManager.ColumnSize, _levelManager.RowSize, _gridManager.transform.position);

            var linkManager = new LinkManager(_gridManager, _linkVisualController);
            _linkInputController.Init(_cameraController.Camera, linkManager);

            _gridManager.PopulateGrid();
        }
    }
}