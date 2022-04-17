using System;
using RoadSimulator.Scripts.Game.LevelEditor;
using RoadSimulator.Scripts.Game.Simulation.Agent;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
    public class Plug : Road
    {
        [SerializeField] private VehicleSpawn spawn;

        public override void SetParams(IRoadParams roadParams)
        {
            if (roadParams is not PlugParams @params)
                throw new Exception($"Expected {typeof(JunctionParams)} but received {roadParams.GetParamsType()}");

            spawn.SetSpawnParams(@params.SpawnParams);

            base.SetParams(roadParams);
        }


        private void OnTriggerEnter(Collider other)
        {
            var vehicle = other.gameObject.GetComponent<Vehicle>();
            if (vehicle != null)
            {
                RegisterVehicle(vehicle, false);
                Destroy(vehicle.gameObject);
            }
        }
    }
}