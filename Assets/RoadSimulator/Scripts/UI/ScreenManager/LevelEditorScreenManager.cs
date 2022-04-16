using System;
using RoadSimulator.Scripts.Game.LevelEditor;
using RoadSimulator.Scripts.Game.Simulation.World;
using RoadSimulator.Scripts.UI.Common;
using RoadSimulator.Scripts.UI.Popup;
using RoadSimulator.Scripts.UI.Utils;
using RoadSimulator.Scripts.UI.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoadSimulator.Scripts.UI.ScreenManager
{
    public class LevelEditorScreenManager : BaseScreenManager, ToolListView.ICallback, LevelEditorInputHandler.ICallback, LevelEditorWorldHolder.ICallback
    {
        [SerializeField] private ToolListView toolListView;

        [SerializeField] private Camera editorCamera;

        [SerializeField] private RoadResourcesHolder resourcesHolder;

        private LevelEditorWorldHolder _levelEditorWorldHolder;
        private CursorPositionChecker _cursorPositionChecker;
        private LevelEditorInputHandler _inputHandler;

        private void Start()
        {
            _cursorPositionChecker = new CursorPositionChecker(safeArea.transform);
            _levelEditorWorldHolder = new LevelEditorWorldHolder(this, resourcesHolder.GetResources());
            _inputHandler = new LevelEditorInputHandler(this);

            toolListView.Init(this, ToolFactory.GetAllTools());
        }

        private void Update()
        {
            _inputHandler.HandleInput();

            if (Input.GetKeyUp(KeyCode.Space))
            {
                editorCamera.transform.position = Vector3.zero;
            }
        }

        public override void OnDialogClosed()
        {
            _levelEditorWorldHolder.UnlockCursor();
        }

        public void OnCloseButtonPressed()
        {
            OpenPopup<YesNoPopup>("Popups/YesNoPopup", new Action(() => { SceneManager.LoadScene("MainScreen"); }));
        }

        public void OnRubbishButtonPressed()
        {
            OpenPopup<YesNoPopup>("Popups/YesNoPopup", new Action(() => { _levelEditorWorldHolder.Reset(); }));
        }

        public void OnRunButtonPressed()
        {
            if (_levelEditorWorldHolder.ReadyToSimulate())
            {
                SimulationWorld.GetInstance().SetRoadData(_levelEditorWorldHolder.GetRoadDataSet());
                SceneManager.LoadScene("SimulationScreen");
            }
        }

        public void OnNewToolSelected(ITool tool)
        {
            _levelEditorWorldHolder.UpdateSelectedTool(tool as IRoadBuilderTool);
        }

        public void OnAllToolsUnselected()
        {
            _levelEditorWorldHolder.SelectBaseTool();
        }

        public void OnPressedMove(Vector3 oldPosition, Vector3 newPosition)
        {
            editorCamera.transform.position += editorCamera.ScreenToWorldPoint(oldPosition) - editorCamera.ScreenToWorldPoint(newPosition);
        }

        public void OnClick(Vector3 position)
        {
            if (_cursorPositionChecker.IsOverGUI(position))
            {
                return;
            }

            _levelEditorWorldHolder.OnClick(editorCamera.ScreenToWorldPoint(position));
        }

        public void OnNewMousePosition(Vector3 position)
        {
            if (_cursorPositionChecker.IsOverGUI(position))
            {
                return;
            }

            _levelEditorWorldHolder.OnNewMousePosition(editorCamera.ScreenToWorldPoint(position));
        }

        public void OnRotate(bool clockwise) => _levelEditorWorldHolder.OnRotate(clockwise);

        public Vector3 OnGetCameraPosition() => editorCamera.transform.position;

        public Ray OnGetRayUnderCursor() => editorCamera.ScreenPointToRay(Input.mousePosition);

        public void OnUpdateRoadParams(IRoadParams roadParams)
        {
            switch (roadParams.GetParamsType())
            {
                case RoadParamsFactory.Type.Default:
                    OpenPopup<DefaultRoadParamsPopup>("Popups/DefaultRoadParamsPopup", roadParams);
                    break;
                case RoadParamsFactory.Type.Junction:
                    OpenPopup<JunctionParamsPopup>("Popups/JunctionParamsPopup", roadParams);
                    break;
                case RoadParamsFactory.Type.Plug:
                    OpenPopup<PlugParamsPopup>("Popups/PlugParamsPopup", roadParams);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}