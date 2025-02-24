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
    public Hand otherHand;
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
        Attachment attachment = heldItem.GetComponent<Attachment>();
        attachment.Hold(this);
    }
    void Release()
    {
        handAnimator.SetBool((handType == HandType.Left ? "left" : "right") + " grab", false);
        if (heldItem == null) return;
        if (otherHand.heldItem == null)
            heldItem.GetComponent<Attachment>().RippleRelease();
        else
            heldItem.GetComponent<Attachment>().ReleaseHand();
        if (heldItem.parent == null || heldItem.parent.GetComponent<Attachment>() == null)
            heldItem.SetParent(null);
        heldItem = null;
    }
}
