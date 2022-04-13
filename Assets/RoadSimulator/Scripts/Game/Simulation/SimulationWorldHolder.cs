using System.Collections.Generic;
using Kawaiiju.Traffic;
using RoadSimulator.Scripts.Game.Base;
using RoadSimulator.Scripts.Game.LevelEditor;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation
{
    public class SimulationWorldHolder
    {
        private readonly RoadObjectFactory _factory;
        private readonly SimulationWorld _world;
        private readonly Transform _roadParent;
        private readonly NavMeshSurface _navMeshSurface;
        private readonly TrafficSystem _trafficSystem;

        private bool _first = true;

        public SimulationWorldHolder(Transform roadParent, RoadObjectFactory.Resources resources, NavMeshSurface navMeshSurface, TrafficSystem trafficSystem)
        {
            _world = SimulationWorld.GetInstance();
            _factory = new RoadObjectFactory(resources);
            _roadParent = roadParent;
            _navMeshSurface = navMeshSurface;
            _trafficSystem = trafficSystem;
        }

        public void Init()
        {
            var enumerator = _world.GetRoadDataEnumerator();
            PlaceRoads(enumerator);
            
            _navMeshSurface.BuildNavMesh();
            _trafficSystem.Init();
        }

        private void PlaceRoads(IEnumerator<LevelEditorRoad.Data> enumerator)
        {
            while (enumerator.MoveNext())
            {
                var data = enumerator.Current;
                CreateRoad(data);
            }
        }

        private void CreateRoad(LevelEditorRoad.Data data)
        {
            var gameObject = _factory.GetRoadObject(data.Type);
            gameObject.transform.position = data.Position;
            gameObject.transform.rotation = data.Rotation;
            gameObject.transform.SetParent(_roadParent);

            if (_first)
            {
                _navMeshSurface.transform.position = data.Position;
                _first = false;
            }
        }
    }
}