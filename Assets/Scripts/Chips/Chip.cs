using UnityEngine;

namespace Chips
{
    public abstract class Chip : MonoBehaviour
    {
        [SerializeField]
        protected SpriteRenderer _spriteRenderer;

        public Vector2Int Position { get; private set; }
        public ChipColorType ColorType { get; protected set; }

        public void SetPosition(Vector3 boardPos, int x, int y)
        {
            transform.position = boardPos + new Vector3(x, y);
            Position = new Vector2Int(x, y);
        }

        public void SetSortOrder(int yValue = 0)
        {
            _spriteRenderer.sortingOrder = yValue;
        }

        public override string ToString()
        {
            return gameObject.name;
        }

        public abstract void Destroy();
    }
}