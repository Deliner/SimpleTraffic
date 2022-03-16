using UnityEngine;

namespace Common
{
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        private ScreenOrientation _currentOrientation = ScreenOrientation.AutoRotation;

        private RectTransform _panelSafeArea;
        private Rect _currentSafeArea;

        private void Start()
        {
            _panelSafeArea = GetComponent<RectTransform>();

            _currentOrientation = Screen.orientation;
            _currentSafeArea = Screen.safeArea;

            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            if (_panelSafeArea == null)
            {
                return;
            }

            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            var pixelRect = canvas.pixelRect;
            anchorMin.x /= pixelRect.width;
            anchorMin.y /= pixelRect.height;

            anchorMax.x /= pixelRect.width;
            anchorMax.y /= pixelRect.height;

            _panelSafeArea.anchorMin = anchorMin;
            _panelSafeArea.anchorMax = anchorMax;

            _currentOrientation = Screen.orientation;
            _currentSafeArea = Screen.safeArea;
        }

        private void Update()
        {
            if (_currentOrientation != Screen.orientation || _currentSafeArea != Screen.safeArea)
            {
                ApplySafeArea();
            }
        }
    }
}