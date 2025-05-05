using System.Collections.Generic;
using Cores;
using UnityEngine;

namespace Tiles
{
    // Manages a pool of Tile instances to create or reset the game board's tile grid.
    public class TilePool : ObjectPool<Tile>
    {
        private readonly Stack<Tile> _tilesOnBoard = new();

        public void CreateTiles(int width, int height, Vector3 boardOrigin)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var tile = GetObject();
                    tile.SetPosition(boardOrigin + new Vector3(x, y, 1));
                    tile.gameObject.SetActive(true);
                    _tilesOnBoard.Push(tile);
                }
            }
        }

        // Deactivates all active tiles and clears the tracking stack.
        public void Reset()
        {
            foreach (var tile in _tilesOnBoard)
            {
                tile.gameObject.SetActive(false);
                ReturnToPool(tile);
            }
            _tilesOnBoard.Clear();
        }
    }
}