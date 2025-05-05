using UnityEngine;

namespace Tiles
{
    // Represents a single board tile 
    public class Tile : MonoBehaviour
    {
        public void SetPosition(Vector3 worldPos)
        {
            transform.position = worldPos;
        }
    }
}
