using System;
using UnityEngine;

namespace Managers
{
    public class GameRuleManager
    {
        private int _remainingMoves;
        private int _currentScore;

        private bool IsGameOver => _remainingMoves <= 0 || _currentScore <= 0;
        private bool HasWon => _currentScore <= 0;

        public event Action<int, int> LinkResolved;
        public event Action<GameStatus> GameFinished;

        public GameRuleManager(int moveAmount, int requiredScore)
        {
            _remainingMoves = moveAmount;
            _currentScore = requiredScore;
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
                GameFinished?.Invoke(HasWon ? GameStatus.Win : GameStatus.Lose);
                Debug.Log(HasWon ? GameStatus.Win : GameStatus.Lose);
            }
        }
    }
}