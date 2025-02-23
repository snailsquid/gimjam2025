using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretItem : MonoBehaviour
{
    public string logPopupText;
    public int secretNumber;
    public float popupTime;
    void Start()
    {
    }

    public void TouchingSecret()
    {
        // Do something then destroy
        LogPopupScript.Instance.ShowPopup(logPopupText, popupTime);
        SecretManager.Instance.AddSecret(secretNumber);
        Destroy(gameObject);
    }
}
