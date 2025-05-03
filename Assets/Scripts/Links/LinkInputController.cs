using Cores;
using UnityEngine;

namespace Links
{
    public class LinkInputController : Singleton<LinkInputController>
    {
        private Camera _camera;
        private int _activeFingerId = -1;
        public LinkManager LinkManager { get; private set; }
        public bool IsLinking { get; private set; }

        public void Init(Camera mainCamera, LinkManager linkManager)
        {
            _camera = mainCamera;
            LinkManager = linkManager;
        }

        void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                HandleTouch(touch);
            }
        }

        private void HandleTouch(Touch touch)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // İlk parmak dokunması
                    _activeFingerId = touch.fingerId;
                    BeginLink(touch.position);
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (touch.fingerId == _activeFingerId)
                        ContinueLink(touch.position);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (touch.fingerId == _activeFingerId)
                        EndLink();
                    break;
            }
        }

        private void HandleMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                BeginLink(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0) && IsLinking)
            {
                ContinueLink(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0) && IsLinking)
            {
                EndLink();
            }
        }

        private void BeginLink(Vector2 screenPos)
        {
            IsLinking = true;
            Vector2 worldPos = _camera.ScreenToWorldPoint(screenPos);
            LinkManager.StartLinkAt(worldPos);
        }

        private void ContinueLink(Vector2 screenPos)
        {
            Vector2 worldPos = _camera.ScreenToWorldPoint(screenPos);
            LinkManager.TryContinueAt(worldPos);
        }

        private void EndLink()
        {
            IsLinking = false;
            LinkManager.EndLink();
            _activeFingerId = -1;
        }
    }
}