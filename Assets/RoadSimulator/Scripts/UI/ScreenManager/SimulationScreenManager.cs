using System;
using RoadSimulator.Scripts.Game.Base;
using RoadSimulator.Scripts.Game.Simulation.RoadSystem;
using RoadSimulator.Scripts.Game.Simulation.World;
using RoadSimulator.Scripts.UI.Common;
using RoadSimulator.Scripts.UI.Popup;
using RoadSimulator.Scripts.UI.Utils;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoadSimulator.Scripts.UI.ScreenManager
{
    public class SimulationScreenManager : BaseScreenManager, InputHandler.ICallback, SimulationWorldHolder.ICallback
    {
        [SerializeField] private RoadResourcesHolder resourcesHolder;
        [SerializeField] private NavMeshSurface navMeshSurface;
        [SerializeField] private Camera simulationCamera;
        [SerializeField] private Transform roadFolder;

        private CursorPositionChecker _positionChecker;
        private SimulationWorldHolder _worldHolder;
        private InputHandler _inputHandler;

        private bool _inputLocked;

        private void Start()
        {
            _worldHolder = new SimulationWorldHolder(this, roadFolder, resourcesHolder.GetResources(), navMeshSurface);
            _worldHolder.Init();

            _positionChecker = new CursorPositionChecker(safeArea.transform);
            _inputHandler = new InputHandler(this);
        }

        private void Update()
        {
            _inputHandler.HandleInput();
        }

        public void OnCloseButtonClicked()
        {
            SimulationWorld.GetInstance().Reset();
            SceneManager.LoadScene("MainScreen");
        }

        public override void OnDialogClosed() => _inputLocked = false;

        public void OnPressedMove(Vector3 oldPosition, Vector3 newPosition)
        {
        }

        public void OnClick(Vector3 position)
        {
            if (_inputLocked)
                return;
            if (_positionChecker.IsOverGUI(position))
                return;

            _worldHolder.OnClick(simulationCamera.ScreenToWorldPoint(position));
        }

        public void OnNewMousePosition(Vector3 position)
        {
        }

        public void OnRotate(bool clockwise)
        {
        }

        public void ShowRoadBandwidth(Road.IBandwidthInformer informer)
        {
            _inputLocked = true;
            OpenPopup<BandwidthPopup>("Popups/BandwidthPopup", informer);
        }

        public Ray OnGetRayUnderCursor() => simulationCamera.ScreenPointToRay(Input.mousePosition);
    }
}