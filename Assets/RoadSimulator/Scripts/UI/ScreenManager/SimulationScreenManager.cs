using System;
using Common;
using RoadSimulator.Scripts.Game.Simulation;
using RoadSimulator.Scripts.Game.Simulation.World;
using RoadSimulator.Scripts.UI.Utils;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoadSimulator.Scripts.UI.ScreenManager
{
    public class SimulationScreenManager : BaseScreenManager
    {

        [SerializeField] private RoadResourcesHolder resourcesHolder;
        [SerializeField] private Transform roadFolder;
        [SerializeField] private NavMeshSurface navMeshSurface;
        [SerializeField] private TrafficSystem trafficSystem;
        
        private SimulationWorldHolder _worldHolder;
        private void Start()
        {
            _worldHolder = new SimulationWorldHolder(roadFolder, resourcesHolder.GetResources(), navMeshSurface, trafficSystem);
            _worldHolder.Init();
        }
        
        public void OnCloseButtonClicked()
        {
            SimulationWorld.GetInstance().Reset();
            SceneManager.LoadScene("MainScreen");
        }
    }
}
