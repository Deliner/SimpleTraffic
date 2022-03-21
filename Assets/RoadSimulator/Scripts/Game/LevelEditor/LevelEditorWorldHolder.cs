using UnityEngine;

namespace Kawaiiju.Traffic.LevelEditor
{
    public class LevelEditorWorldHolder : LevelEditorInputHandler.ICallback
    {
        private static readonly Tool SelectTool = new SelectTool();
        private Tool _currentTool = SelectTool;

        private readonly Camera _camera;
        private readonly IMousePositionChecker _mouseChecker;

        public LevelEditorWorldHolder(Camera camera, IMousePositionChecker mouseChecker)
        {
            _mouseChecker = mouseChecker;
            _camera = camera;
        }

        public void Update()
        {
        }

        public void UpdateSelectedTool(Tool tool)
        {
            _currentTool = tool;
        }

        public void SelectBaseTool()
        {
            _currentTool = SelectTool;
        }

        public void OnClick(Vector3 position)
        {
            if (_mouseChecker.IsOverGUI(position))
            {
                return;
            }
        }

        public void OnPressedMove(Vector3 oldPosition, Vector3 newPosition)
        {
            _camera.transform.position += _camera.ScreenToWorldPoint(oldPosition) - _camera.ScreenToWorldPoint(newPosition);
        }


        public interface IMousePositionChecker
        {
            public bool IsOverGUI(Vector3 position);
        }
    }
}