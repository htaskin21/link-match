using Chips;
using Links;
using Logic;
using Particles;
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

        [SerializeField]
        private ParticleManager _particleManager;

        [SerializeField]
        private UIManager _uiManager;

        private LevelDataSO _currentLevel;
        private GameRuleManager _gameRuleManager;

        private void Start()
        {
            _currentLevel = _levelManager.GetLevelData(0);
            _cameraController.Setup(_currentLevel.RowSize, _currentLevel.ColumnSize);

            _linkableChipPool.Init(_currentLevel.NumberOfColors);
            var poolSize = _currentLevel.ColumnSize * _currentLevel.RowSize * 2;
            _linkableChipPool.CreatePool(poolSize);

            var chipMatcher = new ChipMatcher();
            var gravityController = new GravityController();
            var boardRefiller = new BoardRefiller();
            var boardShuffler = new BoardShuffler();
            _gridManager.Init(_currentLevel.ColumnSize,
                _currentLevel.RowSize, _linkableChipPool, chipMatcher,
                gravityController, boardRefiller, boardShuffler);
            _tileManager.CreateTiles(_currentLevel.ColumnSize, _currentLevel.RowSize, _gridManager.transform.position);

            _gameRuleManager = new GameRuleManager(_currentLevel.MoveAmount, _currentLevel.ReqWinScore);

            var linkManager = new LinkManager(_gridManager, _linkVisualController, _gameRuleManager);
            _linkInputController.Init(_cameraController.Camera, linkManager);

            _particleManager.Init();

            _uiManager.Init(_currentLevel, _gameRuleManager, RestartGame, PlayNextLevel);

            _gridManager.PopulateGrid();
        }

        private void RestartGame()
        {
            _gameRuleManager.Reset(_currentLevel.MoveAmount, _currentLevel.ReqWinScore);
            _gridManager.Shuffle();
            _uiManager.EndGameCanvas.ToggleCanvas(false);
        }

        private void PlayNextLevel()
        {
            _currentLevel = _levelManager.GetRandomLevel();
            _cameraController.Setup(_currentLevel.RowSize, _currentLevel.ColumnSize);
            _gridManager.RemoveAllChips();
           
            _linkableChipPool.Init(_currentLevel.NumberOfColors);
            var poolSize = _currentLevel.ColumnSize * _currentLevel.RowSize * 2;
            _linkableChipPool.IncreasePoolSize(poolSize);

            _gridManager.Init(_currentLevel.ColumnSize,
                _currentLevel.RowSize);

            _tileManager.CreateTiles(_currentLevel.ColumnSize, _currentLevel.RowSize, _gridManager.transform.position);

            _gameRuleManager.Reset(_currentLevel.MoveAmount, _currentLevel.ReqWinScore);
            _gridManager.PopulateGrid();
            _uiManager.EndGameCanvas.ToggleCanvas(false);
        }
    }
}