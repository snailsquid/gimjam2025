using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance {get; private set;}

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public LevelSelectionManager levelSelectionManager;
    public LoadingScreen loadingScreen;
    public static bool wasPlayed;

    public void PlayGame()
    {
        if (wasPlayed == true)
        {
            levelSelectionManager.SelectLevel();
        }
        else
        {
            wasPlayed = true;
            loadingScreen.LoadLevelBtn("MainMenu");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
