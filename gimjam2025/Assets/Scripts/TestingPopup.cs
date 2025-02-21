using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPopup : MonoBehaviour
{
    public void Show()
    {
        LogPopupScript.Instance.ShowDialogue("yot", 2f);
    }    
}
