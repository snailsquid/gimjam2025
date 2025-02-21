using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogsManager : MonoBehaviour
{
    public static LogsManager instance {get; private set;}

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

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject logMenu;

    public void Logs()
    {
        mainMenu.SetActive(false);
        logMenu.SetActive(true);
    }
}
