using System;
using System.Collections.Generic;
using RoadSimulator.Scripts.Game.Simulation.Agent;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
    public class NavSection : MonoBehaviour
    {
        public VehicleSpawn[] vehicleSpawns;
        public NavConnection[] connections;
        public int speedLimit = 120;

        private readonly List<Vehicle> _registeredVehicles = new();


        public bool TryGetVehicleSpawn(out VehicleSpawn spawn)
        {
            var index = UnityEngine.Random.Range(0, vehicleSpawns.Length);
            if (_registeredVehicles.Count == 0 && vehicleSpawns.Length > 0)
            {
                spawn = vehicleSpawns[index];
                return true;
            }

            spawn = null;
            return false;
        }

        public void RegisterVehicle(Vehicle input, bool isAdd)
        {
            if (isAdd)
                _registeredVehicles.Add(input);
            else
            {
                if (_registeredVehicles.Contains(input))
                    _registeredVehicles.Remove(input);
                else
                    Debug.LogWarning("Traffic: Attempted to remove non-existing vehicle from Road: " + gameObject.name);
            }
        }
    }
}