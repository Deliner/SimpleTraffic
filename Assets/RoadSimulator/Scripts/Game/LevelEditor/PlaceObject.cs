using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic.LevelEditor
{
    public class PlaceObject : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] MeshRenderers;

        [SerializeField] private Material Overlapped;
        [SerializeField] private Material Normal;

        public bool isOverlapped { get; private set; }

        private bool _isPlaced;

        private readonly Hashtable _enteredCollider = new();

        public void SetPlaced()
        {
            _isPlaced = true;
            ApplyMaterial(Normal);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isPlaced)
            {
                ApplyMaterial(Overlapped);
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
                    ApplyMaterial(Normal);
                    isOverlapped = false;
                }
            }
        }

        private void ApplyMaterial(Material material)
        {
            foreach (var renderer in MeshRenderers)
            {
                renderer.material = material;
            }
        }
    }
}