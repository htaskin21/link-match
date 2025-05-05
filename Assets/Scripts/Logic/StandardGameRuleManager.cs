using System;
using System.Collections.Generic;
using Chips;

namespace Logic
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

        /// <summary>
        /// Handles completion of a link, deducting moves and score, then raising events.
        /// </summary>
        public void ResolveLink(List<LinkableChip> chips)
        {
            var chipCount = chips.Count;
            if (_remainingMoves <= 0 || chipCount <= 0)
                return;

            _remainingMoves--;
            _currentScore -= chipCount * ScoreMultiplier;
            var correctedScore = (int)MathF.Max(0, _currentScore);
            LinkResolved?.Invoke(_remainingMoves, correctedScore);

            CheckGameStatus();
        }

        private void CheckGameStatus()
        {
            if (IsGameOver)
            {
                GameFinished?.Invoke(HasWon ? GameState.Win : GameState.Lose);
            }
        }

        /// <summary>
        /// Resets remaining moves and score, firing LinkResolved to update UI.
        /// </summary>
        public void Reset(int moveAmount, int startScore)
        {
            _remainingMoves = moveAmount;
            _currentScore = startScore;
            LinkResolved?.Invoke(_remainingMoves, _currentScore);
        }
    }
}