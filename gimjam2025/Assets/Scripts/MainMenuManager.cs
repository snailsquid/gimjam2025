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
    public static bool playedForTheFirstTime = true;

    public void PlayGame()
    {
        if (!playedForTheFirstTime)
        {
            levelSelectionManager.SelectLevel();
        }
        else
        {
            playedForTheFirstTime = false;
            loadingScreen.LoadLevelBtn("MainMenu"); //Change this when first level scene is ready
        }
    }

    

    public void QuitGame()
    {
        Application.Quit();
    }
}
