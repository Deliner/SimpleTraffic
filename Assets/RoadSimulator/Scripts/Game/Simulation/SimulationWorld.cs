using System.Collections.Generic;
using RoadSimulator.Scripts.Game.LevelEditor;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation
{
    public class SimulationWorld
    {
        private static SimulationWorld _instance;

        private HashSet<LevelEditorRoad.Data> _dataSet;

        private SimulationWorld()
        {
        }

        public void SetRoadData(HashSet<LevelEditorRoad.Data> dataSet)
        {
            _dataSet = dataSet;
        }

        public void Reset()
        {
            _dataSet.Clear();
        }

        public IEnumerator<LevelEditorRoad.Data> GetRoadDataEnumerator() => _dataSet.GetEnumerator();

        public static SimulationWorld GetInstance()
        {
            _instance ??= new SimulationWorld();
            return _instance;
        }
    }
}