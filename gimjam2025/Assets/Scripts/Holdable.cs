using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    Rigidbody rigidBody;
    void Start()
    {
        if (GetComponent<Rigidbody>() == null) { gameObject.AddComponent<Rigidbody>(); }
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
        gameObject.tag = "Holdable";
    }
    // void OnCollisionEnter(Collision collision)
    // {
    //     GetComponent<Rigidbody>().isKinematic = false;
    // }
    // void OnCollisionExit(Collision collision)
    // {
    //     GetComponent<Rigidbody>().isKinematic = true;
    // }
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
