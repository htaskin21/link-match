using System.Collections.Generic;
using Cores;
using UnityEngine;

namespace Tiles
{
    public class TilePool : ObjectPool<Tile>
    {
        private readonly Stack<Tile> _tiles = new();

        public void CreateTiles(int width, int height, Vector3 boardOrigin)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var tile = GetObject();
                    tile.SetPosition(boardOrigin + new Vector3(x, y, 1));
                    tile.gameObject.SetActive(true);
                    _tiles.Push(tile);
                }
            }
        }

        public void Reset()
        {
            foreach (var tile in _tiles)
            {
                tile.gameObject.SetActive(false);
            }
            _tiles.Clear();
        }
    }
}