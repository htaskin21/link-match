using System;
using Logic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // Manages the end-of-game panel UI
    public class EndGameCanvas : UICanvas
    {
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

        /// <summary>
        /// Displays the end-game panel with appropriate header and opens the canvas.
        /// </summary>
        public void ShowEndGamePanel(GameState gameState)
        {
            SetEndGamePanel(gameState);
            Open();
        }

        /// <summary>
        /// Sets header text and button visibility based on win or lose state.
        /// </summary>
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