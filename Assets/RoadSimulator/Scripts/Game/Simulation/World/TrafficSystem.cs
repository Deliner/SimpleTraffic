using System;
using System.Collections;
using RoadSimulator.Scripts.Game.Simulation.RoadSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoadSimulator.Scripts.Game.Simulation.World
{
    public class TrafficSystem : MonoBehaviour
    {
        public static TrafficSystem instance => InstanceHolder.trafficSystem;

        public bool drawGizmos;
        public GameObject[] vehiclePrefab;
        public Transform folder;

        private int _vehicleSpawnAttempts;

        private void Start()
        {
            foreach (var spawn in FindObjectsOfType<VehicleSpawn>())
                InitVehicleSpawn(spawn);
        }

        private void InitVehicleSpawn(VehicleSpawn spawn)
        {
            var enumerator = GetEnumeratorFromSpawn(spawn);
            StartCoroutine(enumerator);
        }

        private IEnumerator GetEnumeratorFromSpawn(VehicleSpawn spawn)
        {
            while (true)
            {
                var prefab = vehiclePrefab[Random.Range(0, vehiclePrefab.Length)];
                spawn.TryToSpawn(prefab, folder.transform);
                yield return new WaitForSeconds(spawn.GetDelayBeforeNextSpawn());
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public static float GetAgentSpeedFromKph(int kph)
        {
            return kph * .02f;
        }

        private static class InstanceHolder
        {
            public static TrafficSystem trafficSystem
            {
                get
                {
                    if (_instance == null)
                        _instance = FindObjectOfType<TrafficSystem>();
                    return _instance;
                }
            }

            private static TrafficSystem _instance;
        }
    }
}