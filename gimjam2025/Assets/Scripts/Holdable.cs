using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    Rigidbody rigidBody;
    public Attachment attachment, attachmentPrefab;
    public bool checking;
    void Start()
    {
        if (GetComponent<Rigidbody>() == null) { gameObject.AddComponent<Rigidbody>(); }
        rigidBody = GetComponent<Rigidbody>();
        attachment = GetComponent<Attachment>();
        rigidBody.isKinematic = false;
        rigidBody.drag = 200;
        rigidBody.angularDrag = 200;
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
    void OnTriggerEnter(Collider other)
    {
        Transform hitTransform = other.gameObject.transform;
        if(hitTransform.CompareTag("SubmitingPoint"))
        {
            Debug.Log(gameObject);
            checking = attachment.EqualTo(attachmentPrefab);
        }
    }
}
