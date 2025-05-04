using Cores;
using Logic;
using Managers;
using UnityEngine;

namespace Links
{
    // Handles touch input and delegates link actions to LinkManager 
    public class LinkInputController : Singleton<LinkInputController>
    {
        public bool IsLinking { get; private set; }

        private int _activeFingerId = -1;
        private LinkManager _linkManager;
        private Camera _camera;
        private GameStateManager _gameStateManager;

        public void Init(Camera mainCamera, LinkManager linkManager, GameStateManager gameStateManager)
        {
            _camera = mainCamera;
            _linkManager = linkManager;
            _gameStateManager = gameStateManager;
        }

        private void Update()
        {
            if (Input.touchCount <= 0) return;
            if (_gameStateManager.CurrentGameState != GameState.Playing) return;

            var touch = Input.GetTouch(0);
            HandleTouch(touch);
        }

        /// <summary>
        /// Processes touch phases to begin, continue, or end a link.
        /// </summary>
        private void HandleTouch(Touch touch)
        {
            switch (touch.phase)
            {
                // Track active finger
                case TouchPhase.Began:
                    _activeFingerId = touch.fingerId;
                    BeginLink(touch.position);
                    break;

                // Continue linking on same finger
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (touch.fingerId == _activeFingerId)
                        ContinueLink(touch.position);
                    break;

                // Finish link on lift
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (touch.fingerId == _activeFingerId)
                        EndLink();
                    break;
            }
        }

        /// <summary>
        /// Starts a link session by converting screen to world position.
        /// </summary>
        private void BeginLink(Vector2 screenPos)
        {
            IsLinking = true;
            Vector2 worldPos = _camera.ScreenToWorldPoint(screenPos);
            _linkManager.StartLinkAt(worldPos);
        }

        /// <summary>
        /// Continues link by passing updated world position to LinkManager.
        /// </summary>
        private void ContinueLink(Vector2 screenPos)
        {
            Vector2 worldPos = _camera.ScreenToWorldPoint(screenPos);
            _linkManager.TryContinueAt(worldPos);
        }

        /// <summary>
        /// Ends the link session and resets active finger tracking.
        /// </summary>
        private void EndLink()
        {
            IsLinking = false;
            _linkManager.EndLink();
            _activeFingerId = -1;
        }
    }
}