using UnityEngine;

namespace Tiles
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField]
        private Tile _tilePrefab;

        [SerializeField]
        private Transform _tilesParent;

        public void CreateTiles(int width, int height, Vector3 boardOrigin)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var tile = Instantiate(_tilePrefab, _tilesParent);
                    tile.SetPosition(boardOrigin + new Vector3(x, y, 1));
                }
            }
        }
    }
}