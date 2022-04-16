using RoadSimulator.Scripts.UI.Common;
using UnityEngine.SceneManagement;

namespace RoadSimulator.Scripts.UI.ScreenManager
{
    public class GameScreenManager : BaseScreenManager
    {
        public void OnCloseButtonClicked()
        {
            SceneManager.LoadScene("MainScreen");
        }
    }
}