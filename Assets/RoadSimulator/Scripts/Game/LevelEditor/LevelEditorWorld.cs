using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.LevelEditor
{
    public class LevelEditorWorld
    {
        private const float WorldGridStep = 0.25f;

        private readonly HashSet<Vector2Int> _pendingConnections = new();
        private readonly HashSet<Vector2Int> _closedConnections = new();
        private readonly Hashtable _placeObjects = new();

        private static LevelEditorWorld _instance;

        private LevelEditorWorld()
        {
        }

        public bool LevelIsOk() => _pendingConnections.Count == 0 && _closedConnections.Count > 0;

        public void Reset()
        {
            _pendingConnections.Clear();
            _closedConnections.Clear();
            _placeObjects.Clear();
        }

        public void PlaceRoad(LevelEditorRoad road, out bool isPlaced)
        {
            if (road.isOverlapped || ContainsRoad(road))
            {
                isPlaced = false;
                return;
            }
            Debug.Log($"{road.GetData().Type} placed at {road.GetData().Position}");
            RegisterRoadConnections(road.GetRoadConnections());
            SaveRoad(road);

            road.SetPlaced();
            isPlaced = true;
        }

        public void RemoveRoad(LevelEditorRoad road)
        {
            _placeObjects.Remove(road);
            RemoveRoadConnections(road.GetRoadConnections());
        }

        public HashSet<LevelEditorRoad.Data> GetRoadDataSet()
        {
            var roadDataSet = new HashSet<LevelEditorRoad.Data>();

            foreach (var o in _placeObjects.Values)
            {
                roadDataSet.Add(o is LevelEditorRoad.Data data ? data : default);
            }

            return roadDataSet;
        }

        private bool ContainsRoad(LevelEditorRoad road)
        {
            return _placeObjects.Values.Cast<object>().Contains(road.GetData());
        }

        private void SaveRoad(LevelEditorRoad road)
        {
            _placeObjects.Add(road, road.GetData());
        }

        private void RegisterRoadConnections(IEnumerator connections)
        {
            while (connections.MoveNext())
            {
                var transform = connections.Current as Transform;
                if (transform != null)
                {
                    RegisterConnection(transform);
                }
            }
        }

        private void RemoveRoadConnections(IEnumerator connections)
        {
            while (connections.MoveNext())
            {
                var transform = connections.Current as Transform;
                if (transform != null)
                {
                    RemoveConnection(transform);
                }
            }
        }

        private void RegisterConnection(Transform transform)
        {
            var coords = GetCoordinatesFromTransform(transform);
            if (_pendingConnections.Contains(coords))
            {
                _pendingConnections.Remove(coords);
                _closedConnections.Add(coords);
            }
            else
            {
                _pendingConnections.Add(coords);
            }
        }

        private void RemoveConnection(Transform transform)
        {
            var coords = GetCoordinatesFromTransform(transform);
            if (_pendingConnections.Contains(coords))
            {
                _pendingConnections.Remove(coords);
            }
            else if (_closedConnections.Contains(coords))
            {
                _closedConnections.Remove(coords);
                _pendingConnections.Add(coords);
            }
        }


        public static Vector3 TransformPositionToWorldGrid(Vector3 position)
        {
            var x = GetIntCoordFromFloat(position.x);
            var z = GetIntCoordFromFloat(position.z);
            return new Vector3(x * WorldGridStep, 0, z * WorldGridStep);
        }

        public static Vector2Int TransformPositionToWorldCoord(Vector3 position)
        {
            var coords = new Vector2Int(GetIntCoordFromFloat(position.x), GetIntCoordFromFloat(position.z));
            return coords;
        }

        private static int GetIntCoordFromFloat(float coord) => (int)Math.Round(coord / WorldGridStep);

        private static Vector2Int GetCoordinatesFromTransform(Transform transform)
        {
            var position = transform.TransformPoint(Vector3.zero);
            return TransformPositionToWorldCoord(position);
        }

        public static LevelEditorWorld GetInstance()
        {
            _instance ??= new LevelEditorWorld();
            return _instance;
        }
    }
}