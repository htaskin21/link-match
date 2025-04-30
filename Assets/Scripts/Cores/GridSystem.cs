using System.Collections.Generic;
using UnityEngine;

namespace Cores
{
    public class GridSystem<T> : MonoBehaviour
    {
        public Vector2Int GridSize { get; protected set; }

        private T[,] _gridData;

        protected void CreateGrid()
        {
            _gridData = new T[GridSize.x, GridSize.y];
        }

        public bool CheckBounds(int x, int y)
        {
            return x >= 0 && x < GridSize.x && y >= 0 && y < GridSize.y;
        }

        public bool CheckBounds(Vector2Int position)
        {
            return CheckBounds(position.x, position.y);
        }

        protected bool IsEmpty(int x, int y)
        {
            return EqualityComparer<T>.Default.Equals(_gridData[x, y], default(T));
        }

        public bool IsEmpty(Vector2Int position)
        {
            return IsEmpty(position.x, position.y);
        }

        protected bool PutItemAt(T item, int x, int y, bool allowOverwrite = false)
        {
            if (!allowOverwrite && !IsEmpty(x, y))
                return false;

            _gridData[x, y] = item;
            return true;
        }

        public bool PutItemAt(T item, Vector2Int position, bool allowOverwrite = false)
        {
            return PutItemAt(item, position.x, position.y, allowOverwrite);
        }

        public T GetItemAt(int x, int y)
        {
            return _gridData[x, y];
        }

        public T GetItemAt(Vector2Int position)
        {
            return GetItemAt(position.x, position.y);
        }

        public T RemoveItemAt(int x, int y)
        {
            T temp = _gridData[x, y];
            _gridData[x, y] = default(T);
            return temp;
        }

        protected T RemoveItemAt(Vector2Int position)
        {
            return RemoveItemAt(position.x, position.y);
        }

        public bool MoveItemTo(int x1, int y1, int x2, int y2, bool allowOverwrite = false)
        {
            if (!allowOverwrite && !IsEmpty(x2, y2))
                return false;

            _gridData[x2, y2] = RemoveItemAt(x1, y1);
            return true;
        }

        public bool MoveItemTo(Vector2Int position1, Vector2Int position2, bool allowOverwrite = false)
        {
            return MoveItemTo(position1.x, position1.y, position2.x, position2.y, allowOverwrite);
        }

        public void SwapItemsAt(int x1, int y1, int x2, int y2)
        {
            (_gridData[x1, y1], _gridData[x2, y2]) = (_gridData[x2, y2], _gridData[x1, y1]);
        }

        public void SwapItemsAt(Vector2Int position1, Vector2Int position2)
        {
            SwapItemsAt(position1.x, position1.y, position2.x, position2.y);
        }
    }
}