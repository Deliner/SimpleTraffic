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

        private SpawnParams _spawnParams;

        private readonly HashSet<Collider> _enteredColliders = new();

        public void TryToSpawn(GameObject prefab, Transform parent)
        {
            if (CanSpawn())
            {
                var vehicle = Instantiate(prefab, spawn.position, spawn.rotation, parent).GetComponent<Vehicle>();
                vehicle.Initialize(road, destination, GetVehicleSpeed());
            }
        }
        
        public void SetSpawnParams(SpawnParams spawnParams) => _spawnParams = spawnParams;

        public float GetDelayBeforeNextSpawn() => _spawnParams.setConstantPeriod ? _spawnParams.constantPeriod : Random.Range(_spawnParams.minPeriod, _spawnParams.maxPeriod);

        private int GetVehicleSpeed() => _spawnParams.setConstantSpeed ? _spawnParams.constantSpeed : Random.Range(_spawnParams.minSpeed, _spawnParams.maxSpeed);

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
                constantPeriod = Math.Clamp(constantPeriod, SimulationInfo.MinSpawnPeriod, SimulationInfo.MaxSpawnPeriod);
                maxPeriod = Math.Clamp(maxPeriod, SimulationInfo.MinSpawnPeriod, SimulationInfo.MaxSpawnPeriod);
                minPeriod = Math.Clamp(minPeriod, SimulationInfo.MinSpawnPeriod, SimulationInfo.MaxSpawnPeriod);

                constantSpeed = Math.Clamp(constantSpeed, SimulationInfo.MinCarSpeed, SimulationInfo.MaxCarSpeed);
                maxSpeed = Math.Clamp(maxSpeed, SimulationInfo.MinCarSpeed, SimulationInfo.MaxCarSpeed);
                minSpeed = Math.Clamp(minSpeed, SimulationInfo.MinCarSpeed, SimulationInfo.MaxCarSpeed);
            }
        }
    }
}