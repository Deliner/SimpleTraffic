using System;
using Common;
using Kawaiiju.Traffic.LevelEditor;
using RoadSimulator.Scripts.UI.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelEditorScreenManager : BaseScreenManager, ToolListView.ICallback
{
    [FormerlySerializedAs("toolHolderView")] [SerializeField]
    private ToolListView toolListView;

    [SerializeField] private Camera camera;

    private LevelEditorInputHandler _inputHandler;
    private LevelEditorWorldHolder _levelEditorWorldHolder;
    private CursorPositionChecker _cursorPositionChecker;

    private void Start()
    {
        _cursorPositionChecker = new CursorPositionChecker(safeArea.transform);
        _levelEditorWorldHolder = new LevelEditorWorldHolder(camera, _cursorPositionChecker);
        _inputHandler = new LevelEditorInputHandler(_levelEditorWorldHolder);

        toolListView.Init(this, ToolManager.GetToolList());
    }

    private void Update()
    {
        _inputHandler.HandleInput();
        _levelEditorWorldHolder.Update();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            camera.transform.position = Vector3.zero;
        }
    }

    public void OnCloseButtonPressed()
    {
        OpenPopup<YesNoPopup>("Popups/YesNoPopup", new Action(() => { SceneManager.LoadScene("MainScreen"); }));
    }

    public void OnRunButtonPressed()
    {
        SceneManager.LoadScene("GameScreen");
    }

    public void OnNewToolSelected(Tool tool)
    {
        _levelEditorWorldHolder.UpdateSelectedTool(tool);
    }

    public void OnAllToolsUnselected()
    {
        _levelEditorWorldHolder.SelectBaseTool();
    }
}