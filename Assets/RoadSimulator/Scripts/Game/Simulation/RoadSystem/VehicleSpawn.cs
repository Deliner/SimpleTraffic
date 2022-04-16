using System;
using System.Collections.Generic;
using RoadSimulator.Scripts.Game.Simulation.Agent;
using RoadSimulator.Scripts.Game.Simulation.World;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
    public class VehicleSpawn : MonoBehaviour
    {
        public NavConnection destination;
        public Transform spawn;
        public Road road;

        [SerializeField] private SpawnParams spawnParams;

        private readonly HashSet<Collider> _enteredColliders = new();

        public void TryToSpawn(GameObject prefab, Transform parent)
        {
            if (CanSpawn())
            {
                var vehicle = Instantiate(prefab, spawn.position, spawn.rotation, parent).GetComponent<Vehicle>();
                vehicle.Initialize(road, destination, GetVehicleSpeed());
            }
        }

        public float GetDelayBeforeNextSpawn() => spawnParams.setConstantPeriod ? spawnParams.constantPeriod : Random.Range(spawnParams.minPeriod, spawnParams.maxPeriod);

        private int GetVehicleSpeed() => spawnParams.setConstantSpeed ? spawnParams.constantSpeed : Random.Range(spawnParams.minSpeed, spawnParams.maxSpeed);

        private bool CanSpawn() => _enteredColliders.Count == 0;

        private void OnTriggerEnter(Collider other)
        {
            _enteredColliders.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _enteredColliders.Remove(other);
        }

        [Serializable]
        public class SpawnParams
        {
            public bool setConstantPeriod = true;
            public float constantPeriod = 2;
            public float maxPeriod = 3;
            public float minPeriod = 1;

            public bool setConstantSpeed = true;
            public int constantSpeed = 30;
            public int maxSpeed = 40;
            public int minSpeed = 20;

            public void Validate()
            {
                constantPeriod = Math.Max(Math.Min(constantPeriod, SimulationInfo.MaxSpawnPeriod), SimulationInfo.MinSpawnPeriod);
                maxPeriod = Math.Max(Math.Min(maxPeriod, SimulationInfo.MaxSpawnPeriod), SimulationInfo.MinSpawnPeriod);
                minPeriod = Math.Min(Math.Max(minPeriod, SimulationInfo.MinSpawnPeriod), SimulationInfo.MaxSpawnPeriod);

                constantSpeed = Math.Max(Math.Min(constantSpeed, SimulationInfo.MaxCarSpeed), SimulationInfo.MinCarSpeed);
                maxSpeed = Math.Max(Math.Min(maxSpeed, SimulationInfo.MaxCarSpeed), SimulationInfo.MinCarSpeed);
                minSpeed = Math.Min(Math.Max(minSpeed, SimulationInfo.MinCarSpeed), SimulationInfo.MaxCarSpeed);
            }
        }
    }
}