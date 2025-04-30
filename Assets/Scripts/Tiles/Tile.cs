using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        public void SetPosition(Vector3 worldPos)
        {
            transform.position = worldPos;
        }
    }
}
