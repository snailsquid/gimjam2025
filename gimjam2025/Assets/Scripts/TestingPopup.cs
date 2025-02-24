using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPopup : MonoBehaviour
{
    public void Show()
    {
        LogPopupScript.Instance.ShowPopup("yot", 2f);
    }
}
