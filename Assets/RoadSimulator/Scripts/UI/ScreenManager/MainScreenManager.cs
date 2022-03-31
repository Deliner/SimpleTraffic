using Common;
using UnityEngine.SceneManagement;

public class MainScreenManager : BaseScreenManager
{
    public void OnPlayClicked()
    {
        // OpenPopup<LevelEditorSettingsPopup>("Popups/LevelEditorSettingsPopupPrefab");
        SceneManager.LoadScene("LevelEditorScreen");
    }
}