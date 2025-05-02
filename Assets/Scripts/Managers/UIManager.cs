using Logic;
using UI;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private UpperBarCanvas _upperBarCanvas;

        [SerializeField]
        private EndGameCanvas _endGameCanvas;

        public EndGameCanvas EndGameCanvas => _endGameCanvas;

        public void Init(LevelDataSO levelData, GameRuleManager gameRuleManager, GameStateManager gameStateManager)
        {
            _upperBarCanvas.Init(levelData.MoveAmount, levelData.ReqWinScore, gameRuleManager);
            _endGameCanvas.Init(gameRuleManager, gameStateManager);
        }
    }
}