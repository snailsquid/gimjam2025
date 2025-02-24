using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance { get; private set; }

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

    public static bool playedForTheFirstTime = true;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelection;
    [SerializeField] private GameObject logMenu;
    [SerializeField] private LevelSelectionManager levelSelectionManager;
    [SerializeField] private LoadingScreen loadingScreen;

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

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        levelSelection.SetActive(false);
        logMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
