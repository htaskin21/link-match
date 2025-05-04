using System;
using System.Collections.Generic;
using Chips;

namespace Managers
{
    public class StandardGameRuleManager : IGameRuler
    {
        private int _remainingMoves;
        private int _currentScore;
        private const int ScoreMultiplier = 10;

        private bool IsGameOver => _remainingMoves <= 0 || _currentScore <= 0;
        private bool HasWon => _currentScore <= 0;

        public event Action<int, int> LinkResolved;
        public event Action<GameState> GameFinished;

        public StandardGameRuleManager(int moveAmount, int startScore)
        {
            _remainingMoves = moveAmount;
            _currentScore = startScore;
        }

        public void ResolveLink(List<LinkableChip> chips)
        {
            var chipCount = chips.Count;
            if (_remainingMoves <= 0 || chipCount <= 0)
                return;

            _remainingMoves--;
            _currentScore -= chipCount * ScoreMultiplier;
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