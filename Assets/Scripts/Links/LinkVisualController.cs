using System.Collections.Generic;
using UnityEngine;

namespace Links
{
    [RequireComponent(typeof(LineRenderer))]
    // Manages the LineRenderer to display the link path between chips.
    public class LinkVisualController : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer _lineRenderer;

        private readonly List<Vector3> _positions = new();

        private void Awake()
        {
            _lineRenderer.positionCount = 0;
            _lineRenderer.useWorldSpace = true;
        }

        public void ResetLine()
        {
            _positions.Clear();
            _lineRenderer.positionCount = 0;
        }

        public void AddPoint(Vector3 worldPosition)
        {
            _positions.Add(worldPosition);
            _lineRenderer.positionCount = _positions.Count;
            _lineRenderer.SetPosition(_positions.Count - 1, worldPosition);
        }

        /// <summary>
        /// Removes the last drawn point and updates the line renderer.
        /// </summary>
        public void RemoveLastPoint()
        {
            if (_positions.Count == 0) return;

            _positions.RemoveAt(_positions.Count - 1);
            _lineRenderer.positionCount = _positions.Count;

            for (int i = 0; i < _positions.Count; i++)
            {
                _lineRenderer.SetPosition(i, _positions[i]);
            }
        }
    }
}