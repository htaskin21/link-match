using System.Collections;
using Logic;
using UnityEngine;

namespace Managers
{
    public class EndGameManager : MonoBehaviour
    {
        private IGameRuler _gameRuleManager;
        private UIManager _uiManager;
        private GridManager _gridManager;
        private const float EndGamePanelShowDelay = 0.2f;

        public void Init(IGameRuler gameRuleManager, UIManager uiManager, GridManager gridManager)
        {
            _gameRuleManager = gameRuleManager;
            _uiManager = uiManager;
            _gridManager = gridManager;

            _gameRuleManager.GameFinished += FinishGame;
        }

        /// <summary>
        /// Starts coroutine to show end screen after board is settled.
        /// </summary>
        private void FinishGame(GameState gameState)
        {
            StartCoroutine(StartEndGameRoutine(gameState));
        }

        /// <summary>
        /// Waits for all chips placed and a short delay before showing end UI.
        /// </summary>
        private IEnumerator StartEndGameRoutine(GameState gameState)
        {
            yield return new WaitUntil(() => _gridManager.AreAllChipsPlaced);
            yield return new WaitForSeconds(EndGamePanelShowDelay);
            _uiManager.EndGameCanvas.ShowEndGamePanel(gameState);
        }

        private void OnDestroy()
        {
            _gameRuleManager.GameFinished -= FinishGame;
        }
    }
}