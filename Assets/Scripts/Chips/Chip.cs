using UnityEngine;

namespace Chips
{
    // Base class for all Chip GameObjects
    public abstract class Chip : MonoBehaviour
    {
        public Vector2Int Position { get; private set; }
        public ChipColorType ColorType { get; protected set; }

        [SerializeField]
        protected SpriteRenderer _spriteRenderer;

        public void SetPosition(Vector3 boardPos, int x, int y)
        {
            transform.position = boardPos + new Vector3(x, y);
            Position = new Vector2Int(x, y);
        }

        public void SetSortOrder(int yValue = 0) => _spriteRenderer.sortingOrder = yValue;

        public override string ToString() => gameObject.name;

        public abstract void Disable();
    }
}