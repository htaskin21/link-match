using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndGameCanvas : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private TextMeshProUGUI _headerText;

        [SerializeField]
        private Button _nextLevelButton;

        [SerializeField]
        private Button _restartButton;

        public void Init(GameStateManager gameStateManager)
        {
            _restartButton.onClick.AddListener(gameStateManager.RestartGame);
            _nextLevelButton.onClick.AddListener(gameStateManager.PlayNextLevel);
        }

        public void ShowEndGamePanel(GameState gameState)
        {
            SetEndGamePanel(gameState);
            _canvas.gameObject.SetActive(true);
        }

        private void SetEndGamePanel(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Win:
                    PrepareWinEndGamePanel();
                    break;
                case GameState.Lose:
                    PrepareLoseEndGamePanel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }

        private void PrepareWinEndGamePanel()
        {
            _headerText.text = "WIN";
            _restartButton.gameObject.SetActive(false);
            _nextLevelButton.gameObject.SetActive(true);
        }

        private void PrepareLoseEndGamePanel()
        {
            _headerText.text = "LOSE";
            _restartButton.gameObject.SetActive(true);
            _nextLevelButton.gameObject.SetActive(false);
        }

        public void ToggleCanvas(bool state)
        {
            _canvas.gameObject.SetActive(state);
        }
    }
}