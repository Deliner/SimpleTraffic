using RoadSimulator.Scripts.Game.Simulation.Agent;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
    public class Plug : Road
    {
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