using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    // Abstract base class providing open/close animations for UI Canvas panels.
    public abstract class UICanvas : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        protected Canvas _canvas;

        [SerializeField]
        protected Image _background;

        [SerializeField]
        protected Image _panel;

        [Header("Animation Settings")]
        [SerializeField]
        private float _backgroundFadeDuration = 0.2f;

        [SerializeField]
        [Range(0f, 1f)]
        private float _backgroundTargetAlpha = 0.5f;

        [SerializeField]
        private float _panelScaleDuration = 0.2f;

        private Sequence _closePanelSequence;
        
        protected virtual void PreOpen()
        {
            _background.DOKill();
            _panel.DOKill();
            _closePanelSequence?.Kill();
            
            _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 0f);
            _panel.transform.localScale = Vector3.zero;
            _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, 1f);
            _canvas.gameObject.SetActive(true);
        }
        
        public virtual void Open()
        {
            PreOpen();
            
            _background
                .DOFade(_backgroundTargetAlpha, _backgroundFadeDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _panel.transform
                        .DOScale(Vector3.one, _panelScaleDuration)
                        .SetEase(Ease.OutBack)
                        .SetUpdate(true);
                });
        }
        
        public virtual void Close()
        {
            _background.DOKill();
            _panel.transform.DOKill();
            _closePanelSequence?.Kill();
            
            _closePanelSequence = DOTween.Sequence();
            _closePanelSequence.SetUpdate(true)
                .Append(_panel.transform
                    .DOScale(Vector3.zero, _panelScaleDuration)
                    .SetEase(Ease.InBack))
                .Append(_background
                    .DOFade(0f, _backgroundFadeDuration)
                    .SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    _canvas.gameObject.SetActive(false);
                    ResetVisuals();
                });
        }
        
        protected virtual void ResetVisuals()
        {
            if (_background == null)return;
                _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 0f);

            if (_panel == null) return;
            _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, 1f);
            _panel.transform.localScale = Vector3.zero;
        }
    }
}