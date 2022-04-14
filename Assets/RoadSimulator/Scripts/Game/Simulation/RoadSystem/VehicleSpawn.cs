using System.Collections.Generic;
using RoadSimulator.Scripts.Game.Simulation.Agent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
    public class VehicleSpawn : MonoBehaviour
    {
        public NavConnection destination;
        public Transform spawn;
        public Road road;

        public bool setConstantPeriod = true;
        public float constPeriod = 2;
        public float maxPeriod = 3;
        public float minPeriod = 1;

        public bool setConstantSpeed = true;
        public int constantSpeed = 30;
        public int maxSpeed = 40;
        public int minSpeed = 20;

        private readonly HashSet<Collider> _enteredColliders = new();

        public void TryToSpawn(GameObject prefab, Transform parent)
        {
            if (CanSpawn())
            {
                var vehicle = Instantiate(prefab, spawn.position, spawn.rotation, parent).GetComponent<Vehicle>();
                vehicle.Initialize(road, destination, GetVehicleSpeed());
            }
        }

        public float GetDelayBeforeNextSpawn() => setConstantPeriod ? constPeriod : Random.Range(minPeriod, maxPeriod);

        private int GetVehicleSpeed() => setConstantSpeed ? constantSpeed : Random.Range(minSpeed, maxSpeed);

        private bool CanSpawn() => _enteredColliders.Count == 0;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Enter {this} {other}");
            _enteredColliders.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log($"Exit {this} {other}");
            _enteredColliders.Remove(other);
        }
    }
}