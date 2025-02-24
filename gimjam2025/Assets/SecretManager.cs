using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretManager : MonoBehaviour
{
    public static SecretManager Instance;
    List<int> foundSecret = new List<int>();
    List<string> logPopups = new List<string>{
        "LOGS UNLOCKED, CHECK THE MAIN MENU",
        "NEW LOGS ACQUIRED",
        "NEW LOGS ACQUIRED",
    };
    public bool isLogsUnlocked = false;
    public void AddSecret(int secretNumber)
    {
        foundSecret.Add(secretNumber);
        if (secretNumber == 1)
        {
            isLogsUnlocked = true;
        }
        LogPopupScript.Instance.ShowPopup(logPopups[secretNumber - 1], 3f);
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
