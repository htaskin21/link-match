using Cores;
using UnityEngine;

namespace Links
{
    public class LinkInputController : Singleton<LinkInputController>
    {
        private Camera _camera;
        public LinkManager LinkManager { get; private set; }
        public bool IsLinking { get; private set; }

        public void Init(Camera mainCamera, LinkManager linkManager)
        {
            _camera = mainCamera;
            LinkManager = linkManager;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsLinking = true;
                Vector2 worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                LinkManager.StartLinkAt(worldPos);
            }
            else if (Input.GetMouseButtonUp(0) && IsLinking)
            {
                IsLinking = false;
                LinkManager.EndLink();
            }
        }
    }
}