using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private float loadingTime;
    private bool isLoading;
    private string scene;

    public void LoadLevelBtn(string levelToLoad)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        scene = levelToLoad;
        isLoading = true;
        
        //LoadLevelAsync(levelToLoad);
    }

    void Update()
    {
        if (isLoading)
        {
            if (loadingSlider.value != loadingSlider.maxValue)
            {
                loadingSlider.value += Time.deltaTime/loadingTime;
            }
            else
            {
                SceneManager.LoadSceneAsync(scene);
            }
        }
    }
    /*
    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            yield return null;
        }
    }
    */
}
