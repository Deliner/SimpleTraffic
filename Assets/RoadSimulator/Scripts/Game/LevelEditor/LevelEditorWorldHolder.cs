using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace Kawaiiju.Traffic.LevelEditor
{
    public class LevelEditorWorldHolder : LevelEditorInputHandler.ICallback
    {
        private static readonly Tool DefaultTool = new SelectTool();
        private Tool _currentTool = DefaultTool;

        private readonly IMousePositionChecker _mouseChecker;
        private readonly RoadFactory _roadFactory;
        private readonly Camera _camera;

        private Quaternion _currentObjectRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        private GameObject _cursorObject;

        private Vector3 aboveCameraHeightVector => new(0.0f, _camera.transform.position.y, 0.0f);

        public LevelEditorWorldHolder(Camera camera, IMousePositionChecker mouseChecker, RoadFactory roadFactory)
        {
            _mouseChecker = mouseChecker;
            _roadFactory = roadFactory;
            _camera = camera;
        }

        public void UpdateSelectedTool(Tool tool)
        {
            _currentTool = tool;
            _currentObjectRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

            DestroyCursorObject();
            SetCursorObjectFromTool();
        }

        public void SelectBaseTool()
        {
            _currentTool = DefaultTool;
            DestroyCursorObject();
        }

        private void SetCursorObjectFromTool()
        {
            if (_currentTool is RoadTool roadTool)
            {
                _cursorObject = _roadFactory.GetRoadObject(roadTool.GetRoadType());
                _cursorObject.transform.position += aboveCameraHeightVector;
                _cursorObject.transform.rotation = _currentObjectRotation;
            }
        }

        private void DestroyCursorObject()
        {
            if (_cursorObject != null)
            {
                Object.Destroy(_cursorObject);
                _cursorObject = null;
            }
        }

        public void OnClick(Vector3 position)
        {
            if (_mouseChecker.IsOverGUI(position))
            {
                return;
            }

            ProcessClick(position);
        }

        private void ProcessClick(Vector3 position)
        {
            switch (_currentTool)
            {
                case RoadTool:
                    RoadPlaceClick(position);
                    break;
                case ModifyTool:
                    ModifyClick(position);
                    break;
                case SelectTool:
                    SelectClick(position);
                    break;
            }
        }

        private void RoadPlaceClick(Vector3 position)
        {
            var placeObject = _cursorObject.GetComponent<PlaceObject>();
            if (!placeObject.isOverlapped)
            {
                placeObject.SetPlaced();
                SetCursorObjectFromTool();
            }
        }

        private void ModifyClick(Vector3 position)
        {
        }

        private void SelectClick(Vector3 position)
        {
        }

        public void OnNewMousePosition(Vector3 position)
        {
            if (_mouseChecker.IsOverGUI(position))
            {
                return;
            }

            if (_cursorObject != null)
            {
                _cursorObject.transform.position = LevelEditorWorld.TransformPositionToWorldGrid(_camera.ScreenToWorldPoint(position));
            }
        }

        public void OnRotate(bool clockwise)
        {
            if (_cursorObject != null)
            {
                var oldRotation = _cursorObject.transform.rotation.eulerAngles.y;
                var modifier = clockwise ? 90.0f : -90.0f;
                
                _currentObjectRotation = Quaternion.Euler(0.0f, oldRotation + modifier, 0.0f);
                _cursorObject.transform.rotation = _currentObjectRotation;
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