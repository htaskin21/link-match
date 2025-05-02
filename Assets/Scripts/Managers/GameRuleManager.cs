using UnityEngine;

namespace Managers
{
    public class GameRuleManager
    {
        private int _remainingMoves;
        private int _currentScore;
        private int _requiredScore;

        public bool IsGameOver => _remainingMoves <= 0 || _currentScore >= _requiredScore;
        public bool HasWon => _currentScore >= _requiredScore;
        public int CurrentScore => _currentScore;
        public int RemainingMoves => _remainingMoves;
        
        public GameRuleManager(int moveAmount, int requiredScore)
        {
            _remainingMoves = moveAmount;
            _requiredScore = requiredScore;
            _currentScore = 0;
        }

        public void ResolveLink(int chipCount)
        {
            if (_remainingMoves <= 0 || chipCount <= 0)
                return;

            _remainingMoves--;
            _currentScore += chipCount * 10;

            Debug.Log($"Move Used. Remaining: {_remainingMoves}, Score: {_currentScore}/{_requiredScore}");
        }
    }
}