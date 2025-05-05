using UnityEngine;

namespace Managers
{
    // Adjusts camera position and size to fit the game board dimensions.
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        public Camera Camera => _camera;

        [SerializeField]
        private float _borderSize;

        public void Setup(int row, int column)
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }

            // Center camera on grid
            _camera!.transform.position = new Vector3((float)(column - 1) / 2, (float)(row - 1) / 2, -10);

            // Compute orthographic size to include borders
            var aspectRatio = Screen.width / (float)Screen.height;
            var vertical = (float)row / 2 + _borderSize;
            var horizontal = ((float)column / 2 + _borderSize) / aspectRatio;
            _camera.orthographicSize = (horizontal > vertical) ? horizontal : vertical;
        }
    }
}