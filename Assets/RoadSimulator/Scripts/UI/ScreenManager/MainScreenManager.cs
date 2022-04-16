using RoadSimulator.Scripts.UI.Common;
using UnityEngine.SceneManagement;

namespace RoadSimulator.Scripts.UI.ScreenManager
{
    public class MainScreenManager : BaseScreenManager
    {
        public void OnPlayClicked()
        {
            // OpenPopup<LevelEditorSettingsPopup>("Popups/LevelEditorSettingsPopupPrefab");
            SceneManager.LoadScene("LevelEditorScreen");
        }
    }
}