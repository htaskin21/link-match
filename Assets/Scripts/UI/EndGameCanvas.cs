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

        private GameRuleManager _gameRuleManager;

        public void Init(GameRuleManager gameRuleManager, Action restartCallBack)
        {
            _gameRuleManager = gameRuleManager;
            _gameRuleManager.GameFinished += ShowEndGamePanel;
            _restartButton.onClick.AddListener(restartCallBack.Invoke);
        }

        private void ShowEndGamePanel(GameStatus gameStatus)
        {
            SetEndGamePanel(gameStatus);
            _canvas.gameObject.SetActive(true);
        }

        private void SetEndGamePanel(GameStatus gameStatus)
        {
            switch (gameStatus)
            {
                case GameStatus.Win:
                    PrepareWinEndGamePanel();
                    break;
                case GameStatus.Lose:
                    PrepareLoseEndGamePanel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameStatus), gameStatus, null);
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

        private void OnDestroy()
        {
            _gameRuleManager.GameFinished -= ShowEndGamePanel;
        }
    }
}