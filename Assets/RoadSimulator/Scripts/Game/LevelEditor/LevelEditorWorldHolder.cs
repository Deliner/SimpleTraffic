﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RoadSimulator.Scripts.Game.Base;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RoadSimulator.Scripts.Game.LevelEditor
{
    public class LevelEditorWorldHolder : IRoadBuilder
    {
        private static readonly IRoadBuilderTool DefaultTool = new SelectTool();
        private IRoadBuilderTool _currentTool = DefaultTool;

        private readonly RoadObjectFactory _roadObjectFactory;

        private readonly LevelEditorWorld _world;

        private Quaternion _currentObjectRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        private GameObject _cursorObject;

        private readonly ICallback _callback;
        
        public LevelEditorWorldHolder(ICallback callback, RoadObjectFactory.Resources resources)
        {
            _roadObjectFactory = new RoadObjectFactory(resources);
            _world = LevelEditorWorld.GetInstance();
            _callback = callback;

            if (!_world.IsWorldEmpty())
            {
                RestoreGameObjects();
            }
        }

        public void UpdateSelectedTool(ITool tool)
        {
            if (tool is not IRoadBuilderTool builderTool)
                throw new Exception($"Expected {typeof(IRoadBuilderTool)}");

            _currentTool = builderTool;
            _currentObjectRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

            DestroyCursorObject();
            SetCursorObjectFromTool();
        }

        public void SelectBaseTool()
        {
            _currentTool = DefaultTool;
            DestroyCursorObject();
        }

        public bool ReadyToSimulate() => _world.LevelIsOk();

        public HashSet<LevelEditorRoad.Data> GetRoadDataSet() => _world.GetRoadDataSet();
        
        public void Reset() => _world.ResetWithObjects();

        public void OnClick(Vector3 position)
        {
            _currentTool?.ActionAt(this, LevelEditorWorld.TransformPositionToWorldCoord(position));
        }

        public void OnNewMousePosition(Vector3 position)
        {
            if (_cursorObject != null)
            {
                _cursorObject.transform.position = LevelEditorWorld.TransformPositionToWorldGrid(position);
            }
        }

        public void OnRotate(bool clockwise)
        {
            if (_cursorObject != null)
            {
                RotateObjectOn90(_cursorObject, clockwise);
                _currentObjectRotation = _cursorObject.transform.rotation;
            }
        }

        public void OnAddRoadAt(Vector2Int coord)
        {
            if (_cursorObject.transform.position.y > 0) return; //TODO improve this, fixes bug with dual place

            var placeObject = _cursorObject.GetComponent<LevelEditorRoad>();
            _world.PlaceRoad(placeObject, out var isPlaced);

            if (isPlaced)
                SetCursorObjectFromTool();
        }

        public void OnRemoveRoadAt(Vector2Int coord)
        {
            var roadObject = TryGetObjectUnderCursor();
            if (IsRoadObject(roadObject, out var roadComponent))
            {
                _world.RemoveRoad(roadComponent);
                Object.Destroy(roadObject);
            }
        }

        public void OnRotate90At(bool clockwise, Vector2Int coord)
        {
            var roadObject = TryGetObjectUnderCursor();
            if (IsRoadObject(roadObject, out var roadComponent))
            {
                _world.RemoveRoad(roadComponent);

                RotateObjectOn90(roadObject, clockwise);
                _world.PlaceRoad(roadComponent, out var isPlaced);

                if (!isPlaced)
                {
                    RotateObjectOn90(roadObject, !clockwise);
                    _world.PlaceRoad(roadComponent, out _);
                }
            }
        }

        public void OnSelectAt(Vector2Int coord)
        {
            var roadObject = TryGetObjectUnderCursor();
            if (IsRoadObject(roadObject, out var roadComponent))
            {
                _callback.OnUpdateRoadParams(roadComponent!.GetData().RoadParams);
            }
        }

        private void RestoreGameObjects()
        {
            var dataSet = _world.GetRoadDataSet();
            _world.Reset();

            foreach (var data in dataSet)
            {
                CreateObjectFromData(data);
            }
        }

        private void CreateObjectFromData(LevelEditorRoad.Data data)
        {
            var gameObject = _roadObjectFactory.GetRoadObject(data.Type);
            gameObject.transform.position = data.Position;
            gameObject.transform.rotation = data.Rotation;

            var component = gameObject.GetComponent<LevelEditorRoad>();
            component.UpdateParams(data.RoadParams);

            _world.PlaceRoad(component, out _);
        }

        [CanBeNull]
        private GameObject TryGetObjectUnderCursor()
        {
            if (Physics.Raycast(_callback.OnGetRayUnderCursor(), out var hit) && hit.collider != null)
            {
                var roadObject = hit.collider.gameObject.GetComponent<LevelEditorRoad>();
                if (roadObject != null)
                {
                    return hit.collider.gameObject;
                }
            }

            return null;
        }

        private void SetCursorObjectFromTool()
        {
            var roadType = _currentTool.GetRoadType();
            if (roadType != null)
            {
                _cursorObject = _roadObjectFactory.GetRoadObject(roadType.Value);
                _cursorObject.transform.position = _callback.OnGetCameraPosition();
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

        private static bool IsRoadObject([CanBeNull] GameObject gameObject, [CanBeNull] out LevelEditorRoad roadComponent)
        {
            roadComponent = null;
            if (gameObject == null) return false;
            roadComponent = gameObject.GetComponent<LevelEditorRoad>();
            return roadComponent != null;
        }

        private static void RotateObjectOn90(GameObject gameObject, bool clockwise)
        {
            var oldRotation = gameObject.transform.rotation.eulerAngles.y;
            var modifier = clockwise ? 90.0f : -90.0f;

            var newRotation = Quaternion.Euler(0.0f, oldRotation + modifier, 0.0f);
            gameObject.transform.rotation = newRotation;
        }

        public interface ICallback
        {
            public Vector3 OnGetCameraPosition();
            public Ray OnGetRayUnderCursor();
            public void OnUpdateRoadParams(IRoadParams roadParams);
        }
    }
}