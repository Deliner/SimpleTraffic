using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenManager : BaseScreenManager
{
    public void OnCloseButtonClicked()
    {
        SceneManager.LoadScene("MainScreen");
    }
}