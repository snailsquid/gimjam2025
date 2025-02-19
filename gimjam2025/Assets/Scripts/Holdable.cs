using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    Rigidbody rigidBody;
    void Start()
    {
        if (GetComponent<Rigidbody>() == null) { Debug.LogError("Add rigidbody to " + gameObject.name); return; }
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.drag = 200;
        rigidBody.angularDrag = 200;
    }
    // void OnCollisionEnter(Collision collision)
    // {
    //     GetComponent<Rigidbody>().isKinematic = false;
    // }
    // void OnCollisionExit(Collision collision)
    // {
    //     GetComponent<Rigidbody>().isKinematic = true;
    // }
}
