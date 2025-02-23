using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretItem : MonoBehaviour
{
    public bool holdingSecretItem;
    GameObject secret;
    void Start()
    {
        holdingSecretItem = false;
    }

    void Update()
    {
        if(holdingSecretItem)
        {
            secret = GameObject.FindGameObjectWithTag ("Secret");
            Destroy(secret);
        }
    }
    public void TouchingSecret()
    {
        holdingSecretItem = true;
    }
}
