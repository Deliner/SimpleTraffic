using System.Collections.Generic;
using RoadSimulator.Scripts.Game.LevelEditor;
using RoadSimulator.Scripts.Game.Simulation.Agent;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
    public abstract class NavSection : MonoBehaviour
    {
        public int speedLimit = 120;

        private readonly List<Vehicle> _registeredVehicles = new();


        public virtual void RegisterVehicle(Vehicle input, bool isAdd)
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

        public abstract void SetParams(IRoadParams roadParams);
    }
}