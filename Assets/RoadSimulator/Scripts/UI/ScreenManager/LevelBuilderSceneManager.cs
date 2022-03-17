using Common;
using UnityEngine;

public class LevelBuilderSceneManager : BaseScreenManager, LevelEditorInputHandler.ICallback
{
    [SerializeField] private Camera camera;

    private LevelEditorInputHandler _inputHandler;

    private void Start()
    {
        _inputHandler = new LevelEditorInputHandler(this);
    }

    private void Update()
    {
        _inputHandler.HandleInput();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            camera.transform.position = Vector3.zero;
        }
    }

    public void OnClick(Vector3 position)
    {
        if (PositionIsOverGui(position))
        {
            return;
        }
    }

    public void OnPressedMove(Vector3 oldPosition, Vector3 newPosition)
    {
        camera.transform.position += camera.ScreenToWorldPoint(oldPosition) - camera.ScreenToWorldPoint(newPosition);
    }

    private bool PositionIsOverGui(Vector3 position)
    {
        var i = 0;
        var safeTransform = safeArea.transform;
        while (i < safeArea.transform.childCount)
        {
            if (GetGlobalPosition(safeTransform.GetChild(i).GetComponent<RectTransform>()).Contains(position))
            {
                return true;
            }

            i++;
        }

        return false;
    }

    private static Rect GetGlobalPosition(RectTransform rectTransform)
    {
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }
}