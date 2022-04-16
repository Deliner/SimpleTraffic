﻿using System.Collections.Generic;
using RoadSimulator.Scripts.Game.Base;
using RoadSimulator.Scripts.Game.LevelEditor;
using RoadSimulator.Scripts.Game.Simulation.RoadSystem;
using Unity.AI.Navigation;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.World
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
        }

        private void PlaceRoads(IEnumerator<LevelEditorRoad.Data> enumerator)
        {

            var shift = Vector3.zero;
            var counter = 0;
            while (enumerator.MoveNext())
            {
                shift += enumerator.Current.Position;
                counter++;
            }

            shift /= counter;
            
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                var data = enumerator.Current;
                CreateRoad(data, shift);
            }
        }

        private void CreateRoad(LevelEditorRoad.Data data, Vector3 shift)
        {
            Debug.Log(data.Type);
            var gameObject = _factory.GetRoadObject(data.Type);
            var shiftedPosition = data.Position - shift;
            
            gameObject.transform.position = shiftedPosition;
            gameObject.transform.rotation = data.Rotation;
            gameObject.transform.SetParent(_roadParent);

            gameObject.GetComponent<NavSection>().SetParams(data.RoadParams);

            if (_first)
            {
                _navMeshSurface.transform.position = shiftedPosition;
                _first = false;
            }
        }
    }
}