using Common;

public class MainScreenManager : BaseScreenManager
{
    public void OnPlayClicked()
    {
        OpenPopup<LevelEditorSettingsPopup>("Popups/LevelEditorSettingsPopupPrefab");
    }
}
