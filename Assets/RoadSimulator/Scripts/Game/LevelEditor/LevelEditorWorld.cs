using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Kawaiiju.Traffic.LevelEditor
{
    public class LevelEditorWorld
    {
        private const float WorldGridStep = 0.25f;

        private readonly HashSet<Vector2Int> _pendingConnections = new();
        private readonly HashSet<Vector2Int> _closedConnections = new();


        public bool LevelIsOk() => _pendingConnections.Count == 0 && _closedConnections.Count > 0;

        public void RegisterRoadConnections(IEnumerator connections)
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

        public void RemoveRoadConnections(IEnumerator connections)
        {
            do
            {
                var transform = connections.Current as Transform;
                if (transform != null)
                {
                    RemoveConnection(transform);
                }
            } while (connections.MoveNext());
        }

        private void RegisterConnection(Transform transform)
        {
            var coords = GetCoordinatesFromTransform(transform);
            Debug.Log(coords);
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

        private static Vector2Int GetCoordinatesFromTransform(Transform transform)
        {
            var position = transform.TransformPoint(Vector3.zero);
            Debug.Log(position);
            var coords = new Vector2Int(GetIntCoordFromFloat(position.x), GetIntCoordFromFloat(position.z));
            return coords;
        }

        public static Vector3 TransformPositionToWorldGrid(Vector3 position)
        {
            var x = GetIntCoordFromFloat(position.x);
            var z = GetIntCoordFromFloat(position.z);
            return new Vector3(x * WorldGridStep, 0, z * WorldGridStep);
        }

        private static int GetIntCoordFromFloat(float coord) => (int)Math.Round(coord / WorldGridStep);
    }
}