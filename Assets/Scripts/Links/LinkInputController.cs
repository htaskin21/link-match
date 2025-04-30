using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Links
{
    public class LinkInputController : MonoBehaviour
    {
        public static bool IsLinking { get; private set; }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsLinking = true;
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                LinkManager.Instance.StartLinkAt(worldPos); // YENİ
            }
            else if (Input.GetMouseButtonUp(0) && IsLinking)
            {
                IsLinking = false;
                LinkManager.Instance.EndLink();
            }
        }
    }
}