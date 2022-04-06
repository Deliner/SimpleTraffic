using System.Collections;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.LevelEditor
{
    public class LevelEditorRoad : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] meshRenderers;

        [SerializeField] private Material overlapped;
        [SerializeField] private Material normal;

        [SerializeField] private Transform[] transforms;

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
    }
}