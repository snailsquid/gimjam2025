using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneHandler : MonoBehaviour
{
    public void gotoReset()
    {
        SceneManager.LoadScene("ItemGenerateScene");
    }
    public void gotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
