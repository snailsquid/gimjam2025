using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    Rigidbody rigidBody;
    public Transform heldItem { get; private set; }
    public Animator handAnimator;
    public Transform hb;
    Transform holdableItem;
    public HandType handType;
    [SerializeField] KeyCode holdKey = KeyCode.E;
    public enum HandType { Left, Right }
    bool isHoldable = false;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void OnTriggerStay(Collider collision)
    {
        Transform hitTransform = collision.gameObject.transform;
        if (hitTransform.CompareTag("Holdable"))
        {
            isHoldable = true;
            holdableItem = hitTransform;
        }
        else if (hitTransform.CompareTag("Secret"))
        {
            Debug.Log("Touching Secret");
            //secretItem.TouchingSecret();
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
    public void Hold()
    {
        handAnimator.SetBool((handType == HandType.Left ? "left" : "right") + " grab", true);
        if (holdableItem == null) return;
        if (holdableItem.CompareTag("Secret"))
        {
            holdableItem.GetComponent<SecretItem>().TouchingSecret();
            return;
        }
        if (!isHoldable) return;
        heldItem = holdableItem;
        heldItem.SetParent(hb);
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
        heldItem.GetComponent<Rigidbody>().useGravity = false;
    }
    void Release()
    {
        handAnimator.SetBool((handType == HandType.Left ? "left" : "right") + " grab", false);
        if (heldItem == null) return;
        heldItem.GetComponent<Attachment>().Release();
        heldItem.SetParent(null);
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.GetComponent<Rigidbody>().useGravity = true;
        heldItem = null;
    }
}
