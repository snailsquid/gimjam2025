using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;

    public void Logs()
    {
        mainMenu.SetActive(false);
        logMenu.SetActive(true);
    }

}
