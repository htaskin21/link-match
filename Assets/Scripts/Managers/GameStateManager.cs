using Chips;
using Logic;
using Tiles;

namespace Managers
{
    // Orchestrates transitions between game states like restart and next level.
    public class GameStateManager
    {
        private readonly GameManager _gameManager;
        private readonly GridManager _gridManager;
        private readonly IGameRuler _gameRuleManager;
        private readonly UIManager _uiManager;
        private readonly CameraController _cameraController;
        private readonly LinkableChipPool _linkableChipPool;
        private readonly TilePool _tilePool;
        private readonly LevelManager _levelManager;

        public GameState CurrentGameState { get; private set; } = GameState.Preparing;


        public GameStateManager(GameManager gameManager, GridManager gridManager, IGameRuler gameRuleManager,
            UIManager uiManager,
            CameraController cameraController, LinkableChipPool linkableChipPool, TilePool tilePool)
        {
            _gameManager = gameManager;
            _gameRuleManager = gameRuleManager;
            _gridManager = gridManager;
            _uiManager = uiManager;
            _cameraController = cameraController;
            _linkableChipPool = linkableChipPool;
            _tilePool = tilePool;

            _gameRuleManager.GameFinished += SetGameState;
        }

        /// <summary>
        /// Resets board and UI to initial level state, then starts playing.
        /// </summary>
        public void RestartGame()
        {
            CurrentGameState = GameState.Preparing;
            var levelDataSo = _gameManager.CurrentLevel;
            _gameRuleManager.Reset(levelDataSo.MoveAmount, levelDataSo.ReqWinScore);
            _gridManager.ShuffleUntilMatch();
            _uiManager.EndGameCanvas.ToggleCanvas(false);
            CurrentGameState = GameState.Playing;
        }

        /// <summary>
        /// Advances to a random next level and reinitializes all components.
        /// </summary>
        public void PlayNextLevel()
        {
            CurrentGameState = GameState.Preparing;
            _gameManager.SetRandomLevel();
            var levelDataSo = _gameManager.CurrentLevel;

            _cameraController.Setup(levelDataSo.RowSize, levelDataSo.ColumnSize);
            _gridManager.ClearGrid();

            _linkableChipPool.Init(levelDataSo.NumberOfColors);
            var poolSize = levelDataSo.ColumnSize * levelDataSo.RowSize * 2;
            _linkableChipPool.IncreasePoolSize(poolSize);

            _gridManager.ResizeGrid(levelDataSo.ColumnSize,
                levelDataSo.RowSize);

            _tilePool.Reset();
            var tilePoolSize = levelDataSo.ColumnSize * levelDataSo.RowSize;
            _tilePool.IncreasePoolSize(tilePoolSize);
            _tilePool.CreateTiles(levelDataSo.ColumnSize,
                levelDataSo.RowSize, _gridManager.transform.position);

            _gameRuleManager.Reset(levelDataSo.MoveAmount, levelDataSo.ReqWinScore);
            _gridManager.PopulateGrid();
            _uiManager.EndGameCanvas.ToggleCanvas(false);
            CurrentGameState = GameState.Playing;
        }

        public void SetGameState(GameState gameState)
        {
            CurrentGameState = gameState;
        }
    }
}