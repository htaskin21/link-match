using System;

namespace Managers
{
    public class GameRuleManager
    {
        private EndGameManager _endGameManager;
        private GameStateManager _gameStateManager;
        
        private int _remainingMoves;
        private int _currentScore;

        private bool IsGameOver => _remainingMoves <= 0 || _currentScore <= 0;
        private bool HasWon => _currentScore <= 0;

        public event Action<int, int> LinkResolved;
        public event Action<GameState> GameFinished;

        public GameRuleManager(int moveAmount, int startScore)
        {
            _remainingMoves = moveAmount;
            _currentScore = startScore;
        }

        public void ResolveLink(int chipCount)
        {
            if (_remainingMoves <= 0 || chipCount <= 0)
                return;

            _remainingMoves--;
            _currentScore -= chipCount * 10;
            LinkResolved?.Invoke(_remainingMoves, _currentScore);

            CheckGameStatus();
        }

        private void CheckGameStatus()
        {
            if (IsGameOver)
            {
                GameFinished?.Invoke(HasWon ? GameState.Win : GameState.Lose);
            }
        }

        public void Reset(int moveAmount, int startScore)
        {
            _remainingMoves = moveAmount;
            _currentScore = startScore;
            LinkResolved?.Invoke(_remainingMoves, _currentScore);
        }
    }
}