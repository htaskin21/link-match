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
        [Header("Level")]
        [SerializeField]
        private LevelManager _levelManager;

        [Header("Tile & Grid")]
        [SerializeField]
        private GridManager _gridManager;

        [SerializeField]
        private TilePool _tilePool;

        [Header("Chips & Links")]
        [SerializeField]
        private LinkableChipPool _linkableChipPool;

        [SerializeField]
        private LinkInputController _linkInputController;

        [SerializeField]
        private LinkVisualController _linkVisualController;

        [Header("UI")]
        [SerializeField]
        private UIManager _uiManager;

        [Header("Effects & Particles")]
        [SerializeField]
        private ParticleManager _particleManager;

        [Header("Grid")]
        [SerializeField]
        private CameraController _cameraController;

        // Runtime 
        public LevelDataSO CurrentLevel { get; private set; }
        private GameRuleManager _gameRuleManager;
        private IChipMatcher _chipMatcher;
        private BoardShuffler _boardShuffler;
        private GameStateManager _gameStateManager;

        private void Start()
        {
            CurrentLevel = _levelManager.GetLevelData(0);
            _cameraController.Setup(CurrentLevel.RowSize, CurrentLevel.ColumnSize);
            CreatePools();
            InitializeBoard();
            InitializeTile();
            InitializeLinkLogic();
            _gameStateManager = new GameStateManager(this, _gridManager, _gameRuleManager, _uiManager,
                _cameraController, _linkableChipPool, _tilePool);
            _uiManager.Init(CurrentLevel, _gameRuleManager, _gameStateManager);
            _gridManager.PopulateGrid();
        }

        private void CreatePools()
        {
            _linkableChipPool.Init(CurrentLevel.NumberOfColors);
            var poolSize = CurrentLevel.ColumnSize * CurrentLevel.RowSize * 2;
            _linkableChipPool.CreatePool(poolSize);

            _particleManager.Init();
        }

        private void InitializeBoard()
        {
            var chipMatcher = new ChipMatcher();
            var gravityController = new GravityController();
            var boardRefiller = new BoardRefiller();
            var boardShuffler = new BoardShuffler();
            _gridManager.Init(CurrentLevel.ColumnSize,
                CurrentLevel.RowSize, _linkableChipPool, chipMatcher,
                gravityController, boardRefiller, boardShuffler);
        }

        private void InitializeTile()
        {
            var poolSize = CurrentLevel.ColumnSize * CurrentLevel.RowSize;
            _tilePool.CreatePool(poolSize);
            _tilePool.CreateTiles(CurrentLevel.ColumnSize,
                CurrentLevel.RowSize, _gridManager.transform.position);
        }

        private void InitializeLinkLogic()
        {
            _gameRuleManager = new GameRuleManager(CurrentLevel.MoveAmount, CurrentLevel.ReqWinScore);
            var linkManager = new LinkManager(_gridManager, _linkVisualController, _gameRuleManager);
            _linkInputController.Init(_cameraController.Camera, linkManager);
        }

        public void SetRandomLevel()
        {
            CurrentLevel = _levelManager.GetRandomLevel();
        }
    }
}