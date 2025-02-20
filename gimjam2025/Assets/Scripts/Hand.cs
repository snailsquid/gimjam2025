using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    Rigidbody rigidBody;
    public Transform heldItem { get; private set; }
    public Transform hb;
    Transform holdableItem;
    [SerializeField] KeyCode holdKey = KeyCode.E;
    bool isHoldable = false;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void OnTriggerEnter(Collider collision)
    {
        Transform hitTransform = collision.gameObject.transform;
        Debug.Log(hitTransform.tag);
        if (hitTransform.CompareTag("Holdable"))
        {
            Debug.Log("Touching");
            isHoldable = true;
            holdableItem = hitTransform;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        holdableItem = null;
        isHoldable = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(holdKey))
            Hold();
        if (Input.GetKeyUp(holdKey))
            Release();
    }
    void Hold()
    {

        if (!isHoldable || holdableItem == null) return;
        heldItem = holdableItem;
        heldItem.SetParent(hb);
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
        heldItem.GetComponent<Rigidbody>().useGravity = false;
    }
    void Release()
    {
        if (heldItem == null) return;
        heldItem.SetParent(null);
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.GetComponent<Rigidbody>().useGravity = true;
        heldItem = null;
    }
}
