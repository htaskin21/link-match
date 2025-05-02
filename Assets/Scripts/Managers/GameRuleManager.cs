using System;
using UnityEngine;

namespace Managers
{
    public class GameRuleManager
    {
        private readonly int _moveAmount;
        private readonly int _startingScore;

        private int _remainingMoves;
        private int _currentScore;

        private bool IsGameOver => _remainingMoves <= 0 || _currentScore <= 0;
        private bool HasWon => _currentScore <= 0;

        public event Action<int, int> LinkResolved;
        public event Action<GameStatus> GameFinished;

        public GameRuleManager(int moveAmount, int startScore)
        {
            _moveAmount = moveAmount;
            _startingScore = startScore;
            
            _remainingMoves = _moveAmount;
            _currentScore = _startingScore;
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

        public void Reset(int moveAmount, int startScore)
        {
            _remainingMoves = moveAmount;
            _currentScore = startScore;
            LinkResolved?.Invoke(_remainingMoves, _currentScore);
        }
    }
}