using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public void Show()
    {
        DialogueBoxScript.Instance.ShowDialogue("yot", 2f);
    }    
}
