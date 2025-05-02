using System;
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

        public void Init(LevelManager levelManager, GameRuleManager gameRuleManager, Action restartCallBack)
        {
            _upperBarCanvas.Init(levelManager.MoveAmount, levelManager.ReqWinScore, gameRuleManager);
            _endGameCanvas.Init(gameRuleManager, restartCallBack);
        }
    }
}