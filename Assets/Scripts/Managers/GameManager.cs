using Chips;
using Links;
using Logic;
using Particles;
using Tiles;
using UnityEngine;

namespace Managers
{
    // Entry point that wires up all systems and starts the game
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

        [SerializeField]
        private EndGameManager _endGameManager;


        private GameStateManager _gameStateManager;
        public LevelDataSO CurrentLevel { get; private set; }
        private IGameRuler _gameRuleManager;
        private IChipMatcher _chipMatcher;
        private BoardShuffler _boardShuffler;


        // Initializes all services and begins the first level.
        private void Start()
        {
            CurrentLevel = _levelManager.GetLevelData(0);
            _cameraController.Setup(CurrentLevel.RowSize, CurrentLevel.ColumnSize);
            CreatePools();
            InitializeBoard();
            InitializeTile();
            _gameRuleManager = new StandardGameRuleManager(CurrentLevel.MoveAmount, CurrentLevel.ReqWinScore);
            _gameStateManager = new GameStateManager(this, _gridManager, _gameRuleManager, _uiManager,
                _cameraController, _linkableChipPool, _tilePool);
            InitializeLinkLogic();
        
            _uiManager.Init(CurrentLevel, _gameRuleManager, _gameStateManager);
            _endGameManager.Init(_gameRuleManager, _uiManager, _gridManager);
            _gridManager.PopulateGrid();
            _gameStateManager.SetGameState(GameState.Playing);
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
            var chipMatcher = new FourWayChipMatcher();
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
            var linkManager = new LinkManager(_gridManager, _linkVisualController, _gameRuleManager);
            _linkInputController.Init(_cameraController.Camera, linkManager, _gameStateManager);
        }

        public void SetRandomLevel()
        {
            CurrentLevel = _levelManager.GetRandomLevel();
        }
    }
}