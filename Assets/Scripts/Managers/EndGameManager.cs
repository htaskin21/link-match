using System.Collections;
using UnityEngine;

namespace Managers
{
    public class EndGameManager : MonoBehaviour
    {
        private IGameRuler _gameRuleManager;
        private UIManager _uiManager;
        private GridManager _gridManager;

        public void Init(IGameRuler gameRuleManager, UIManager uiManager, GridManager gridManager)
        {
            _gameRuleManager = gameRuleManager;
            _uiManager = uiManager;
            _gridManager = gridManager;

            _gameRuleManager.GameFinished += FinishGame;
        }

        private void FinishGame(GameState gameState)
        {
            StartCoroutine(StartEndGameRoutine(gameState));
        }

        IEnumerator StartEndGameRoutine(GameState gameState)
        {
            yield return new WaitUntil(() => _gridManager.AreAllChipsPlaced);
            yield return new WaitForSeconds(0.25f);
            _uiManager.EndGameCanvas.ShowEndGamePanel(gameState);
        }

        private void OnDestroy()
        {
            _gameRuleManager.GameFinished -= FinishGame;
        }
    }
}