using System.Collections.Generic;
using JetBrains.Annotations;
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
        private readonly ICallback _callback;

        private bool _first = true;

        public SimulationWorldHolder(ICallback callback, Transform roadParent, RoadObjectFactory.Resources resources, NavMeshSurface navMeshSurface)
        {
            _factory = new RoadObjectFactory(resources);
            _world = SimulationWorld.GetInstance();

            _navMeshSurface = navMeshSurface;
            _roadParent = roadParent;
            _callback = callback;
        }

        public void Init()
        {
            var enumerator = _world.GetRoadDataEnumerator();
            PlaceRoads(enumerator);

            _navMeshSurface.BuildNavMesh();
        }

        public void OnClick(Vector3 position)
        {
            var roadComponent = TryGetObjectUnderCursor();
            if (roadComponent != null)
                _callback.ShowRoadBandwidth(roadComponent.informer);
        }


        [CanBeNull]
        private Road TryGetObjectUnderCursor()
        {
            var hits = Physics.RaycastAll(_callback.OnGetRayUnderCursor());
            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    var roadObject = hit.collider.gameObject.GetComponentInParent<Road>();
                    if (roadObject != null)
                    {
                        return roadObject;
                    }
                }
            }

            return null;
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

        public interface ICallback
        {
            public void ShowRoadBandwidth(Road.IBandwidthInformer informer);
            public Ray OnGetRayUnderCursor();
        }
    }
}