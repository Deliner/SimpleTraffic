using System.Collections;
using RoadSimulator.Scripts.Game.Base;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.LevelEditor
{
    public class LevelEditorRoad : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] meshRenderers;

        [SerializeField] private Material overlapped;
        [SerializeField] private Material normal;

        [SerializeField] private Transform[] transforms;

        [SerializeField] private RoadObjectFactory.Type type;

        public bool isOverlapped { get; private set; }

        private bool _isPlaced;

        private readonly Hashtable _enteredCollider = new();
        
        public void SetPlaced()
        {
            _isPlaced = true;
            ApplyMaterial(normal);
        }

        public IEnumerator GetRoadConnections()
        {
            return transforms.GetEnumerator();
        }

        public Data GetData()
        {
            var objectTransform = transform;
            return new Data(type, objectTransform.position, objectTransform.rotation);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isPlaced)
            {
                ApplyMaterial(overlapped);
                isOverlapped = true;

                if (_enteredCollider.ContainsKey(other))
                {
                    var value = _enteredCollider[other] is int ? (int)_enteredCollider[other] : 0;
                    _enteredCollider.Remove(other);
                    _enteredCollider.Add(other, value + 1);
                }
                else
                {
                    _enteredCollider.Add(other, 1);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_isPlaced)
            {
                if ((_enteredCollider[other] is int ? (int)_enteredCollider[other] : 0) == 1)
                {
                    _enteredCollider.Remove(other);
                }
                else
                {
                    var value = _enteredCollider[other] is int ? (int)_enteredCollider[other] : 0;
                    _enteredCollider.Remove(other);
                    _enteredCollider.Add(other, value - 1);
                }


                if (_enteredCollider.Count == 0)
                {
                    ApplyMaterial(normal);
                    isOverlapped = false;
                }
            }
        }

        private void ApplyMaterial(Material material)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = material;
            }
        }

        public struct Data
        {
            public readonly RoadObjectFactory.Type Type;
            public readonly Quaternion Rotation;
            public readonly Vector3 Position;

            public Data(RoadObjectFactory.Type type, Vector3 position, Quaternion rotation)
            {
                Position = position;
                Rotation = rotation;
                Type = type;
            }
        }
    }
}