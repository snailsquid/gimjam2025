using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretManager : MonoBehaviour
{
    public static SecretManager Instance;
    List<int> foundSecret = new List<int>();
    public void AddSecret(int secretNumber)
    {
        foundSecret.Add(secretNumber);
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
