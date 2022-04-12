using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
    public enum TrafficType
    {
        Pedestrian,
        Vehicle
    }

    public class TrafficSystem : MonoBehaviour
    {
        public static TrafficSystem instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<TrafficSystem>();
                return _instance;
            }
        }

        private static TrafficSystem _instance;

        public bool drawGizmos;
        public GameObject pedestrianPrefab; // TODO - Get from object pool
        public GameObject vehiclePrefab; // TODO - Get from object pool
        public GameObject trainPrefab; // TODO - Get from object pool
        public GameObject trainCarriagePrefab; // TODO - Get from object pool
        public Transform pool; // TODO - Get from object pool
        public bool spawnOnStart = true;
        public int maxRoadVehicles = 100;
        public int maxTrains = 5;
        public int maxPedestrians = 100;

        private List<Road> m_Roads = new();
        private List<Track> m_Tracks = new();

        private int m_RoadVehicleSpawnAttempts;
        private int m_TrainSpawnAttempts;
        private int m_PedestrianSpawnAttempts;

        private bool inited = false;

        private void Start()
        {
        }

        public void Init()
        {
            var k = kek();
            StartCoroutine(k);
        }
        
        private IEnumerator kek()
        {
            yield return new WaitForSeconds(1);
            var roadsFound = FindObjectsOfType<Road>();
            foreach (var r in roadsFound)
                m_Roads.Add(r);

            for (var i = 0; i < maxRoadVehicles; i++)
                SpawnRoadVehicle(true);

            inited = true;
        }

        private void Update()
        {
            if (inited)
            {
                // if (Input.GetKeyUp(KeyCode.Backspace))
                //     SpawnPedestrian(true);
                if (Input.GetKeyUp(KeyCode.Return))
                    SpawnRoadVehicle(true);
                // if (Input.GetKeyUp(KeyCode.RightShift))
                //     SpawnTrain(true);  
            }
        }

        private void SpawnRoadVehicle(bool reset)
        {
            if (reset)
                m_RoadVehicleSpawnAttempts = 0;
            int index = Random.Range(0, m_Roads.Count);
            Road road = m_Roads[index];
            VehicleSpawn spawn;
            if (!road.TryGetVehicleSpawn(out spawn))
            {
                m_RoadVehicleSpawnAttempts++;
                if (m_RoadVehicleSpawnAttempts < m_Roads.Count)
                    SpawnRoadVehicle(false);
                return;
            }

            Vehicle newVehicle = Instantiate(vehiclePrefab, spawn.spawn.position, spawn.spawn.rotation, pool.transform).GetComponent<Vehicle>();
            newVehicle.Initialize(road, spawn.destination);
        }

        private void SpawnTrain(bool reset)
        {
            if (reset)
                m_TrainSpawnAttempts = 0;
            int index = Random.Range(0, m_Tracks.Count);
            Track track = m_Tracks[index];
            VehicleSpawn spawn;
            if (!track.TryGetVehicleSpawn(out spawn))
            {
                m_TrainSpawnAttempts++;
                if (m_TrainSpawnAttempts < m_Tracks.Count)
                    SpawnRoadVehicle(false);
                return;
            }

            Train newTrain = Instantiate(trainPrefab, spawn.spawn.position, spawn.spawn.rotation, pool.transform).GetComponent<Train>();
            newTrain.Initialize(track, spawn.destination);
        }

        private void SpawnPedestrian(bool reset)
        {
            if (reset)
                m_PedestrianSpawnAttempts = 0;
            int index = Random.Range(0, m_Roads.Count);
            Road road = m_Roads[index];
            Transform spawn;
            if (!road.TryGetPedestrianSpawn(out spawn))
            {
                m_PedestrianSpawnAttempts++;
                if (m_PedestrianSpawnAttempts < m_Roads.Count)
                    SpawnPedestrian(false);
                return;
            }

            Agent newAgent = Instantiate(pedestrianPrefab, spawn.position, spawn.rotation, pool.transform).GetComponent<Agent>();
            newAgent.Initialize();
        }

        public Transform GetPedestrianDestination()
        {
            int index = UnityEngine.Random.Range(0, m_Roads.Count);
            Road road = m_Roads[index];
            Transform destination;
            if (!road.TryGetPedestrianSpawn(out destination))
            {
                return GetPedestrianDestination();
            }

            return destination;
        }

        public float GetAgentSpeedFromKPH(int kph)
        {
            return kph * .02f;
        }
    }
}