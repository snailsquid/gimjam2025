using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretItem : MonoBehaviour
{
    public string logPopupText;
    public int secretNumber;
    public float popupTime;
    public float destroyHeight = -2f;
    void Start()
    {
        gameObject.tag = "Secret";
    }

    public void TouchingSecret()
    {
        // Do something then destroy
        LogPopupScript.Instance.ShowPopup(logPopupText, popupTime);
        SecretManager.Instance.AddSecret(secretNumber);
        Destroy(gameObject);
    }
    private void Update()
    {
        if (transform.position.y < destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}
