using Logic;
using TMPro;
using UnityEngine;

namespace UI
{
    // Displays and updates the remaining moves and current score in the HUD.
    public class UpperBarCanvas : MonoBehaviour
    {
        [Header("Move Panel")]
        [SerializeField]
        private TextMeshProUGUI _movePanelHeaderText;

        [SerializeField]
        private TextMeshProUGUI _movePanelAmountText;

        [Header("Score Panel")]
        [SerializeField]
        private TextMeshProUGUI _scorePanelHeaderText;

        [SerializeField]
        private TextMeshProUGUI _scorePanelAmountText;

        private IGameRuler _gameRuleManager;

        public void Init(int moveAmount, int reqScore, IGameRuler gameRuleManager)
        {
            UpdateAmounts(moveAmount, reqScore);
            _gameRuleManager = gameRuleManager;
            _gameRuleManager.LinkResolved += UpdateAmounts;
        }

        private void UpdateAmounts(int moveAmount, int reqScore)
        {
            _movePanelAmountText.text = moveAmount.ToString();
            _scorePanelAmountText.text = reqScore.ToString();
        }

        private void OnDestroy()
        {
            _gameRuleManager.LinkResolved -= UpdateAmounts;
        }
    }
}