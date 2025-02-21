using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TVManager : MonoBehaviour
{
    public enum tvState {Tutorial,Preview,Submit}
    public Transform spinningItem;
    public float spinningSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        spinningItem.transform.Rotate(spinningSpeed,0,0);
    }
}
