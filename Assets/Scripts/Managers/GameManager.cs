using Chips;
using Data;
using GridFlow;
using Links;
using Logic;
using Matchers;
using Particles;
using Tiles;
using UnityEngine;

namespace Managers
{
    // Entry point that wires up all systems and starts the game
    public class GameManager : MonoBehaviour
    {
        [Header("Core Managers")]
        [SerializeField]
        private LevelManager _levelManager;

        [SerializeField]
        private GridManager _gridManager;

        [SerializeField]
        private CameraController _cameraController;

        [SerializeField]
        private TilePool _tilePool;

        [Header("Chips & Links")]
        [SerializeField]
        private LinkableChipPool _linkableChipPool;

        [SerializeField]
        private LinkInputController _linkInputController;

        [SerializeField]
        private LinkVisualController _linkVisualController;

        [Header("UI & VFX")]
        [SerializeField]
        private UIManager _uiManager;

        [SerializeField]
        private ParticleManager _particleManager;

        [SerializeField]
        private EndGameManager _endGameManager;

        private GameStateManager _gameStateManager;
        private IGameRuler _gameRuleManager;
        private IChipMatcher _chipMatcher;
        private BoardShuffler _boardShuffler;
        public LevelDataSO CurrentLevel { get; private set; }

        private void Awake()
        {
            Application.targetFrameRate = 120;
        }

        // Initializes all services and begins the first level.
        private void Start()
        {
            CurrentLevel = _levelManager.GetLevelData(0);
            SetupCamera();
            InitializePools();
            InitializeBoard();
            InitializeTiles();
            InitializeLinkLogic();
            InitializeUI();
            StartGame();
        }

        public void SetRandomLevel()
        {
            CurrentLevel = _levelManager.GetRandomLevel();
        }

        private void SetupCamera()
        {
            _cameraController.Setup(
                CurrentLevel.RowSize,
                CurrentLevel.ColumnSize
            );
        }

        private void InitializePools()
        {
            _linkableChipPool.Init(CurrentLevel.NumberOfDifferentChips);
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

        private void InitializeTiles()
        {
            var poolSize = CurrentLevel.ColumnSize * CurrentLevel.RowSize;
            _tilePool.CreatePool(poolSize);
            _tilePool.CreateTiles(CurrentLevel.ColumnSize,
                CurrentLevel.RowSize, _gridManager.transform.position);
        }

        private void InitializeLinkLogic()
        {
            _gameRuleManager = new StandardGameRuleManager(CurrentLevel.MoveAmount, CurrentLevel.ReqWinScore);
            _gameStateManager = new GameStateManager(this, _gridManager, _gameRuleManager, _uiManager,
                _cameraController, _linkableChipPool, _tilePool);
            var linkManager = new LinkManager(_gridManager, _linkVisualController, _gameRuleManager);
            _linkInputController.Init(_cameraController.Camera, linkManager, _gameStateManager);
        }

        private void InitializeUI()
        {
            _uiManager.Init(CurrentLevel, _gameRuleManager, _gameStateManager);
            _endGameManager.Init(_gameRuleManager, _uiManager, _gridManager);
        }

        private void StartGame()
        {
            _gridManager.PopulateGrid();
            _gameStateManager.SetGameState(GameState.Playing);
        }
    }
}