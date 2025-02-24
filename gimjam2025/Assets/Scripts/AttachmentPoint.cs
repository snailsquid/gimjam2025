using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class AttachmentPoint : MonoBehaviour
{
    float snapDistance = 0.3f, snapRotation = 16f;
    [SerializeField] public AttachmentType type;
    public AttachmentPoint attachedPoint { get; private set; }
    public Attachment attachment { get; private set; }
    public bool isSnapped;
    Collider componentCollider;
    Rigidbody componentRigidbody;
    FixedJoint componentJoint;
    void Awake()
    {
        transform.tag = "AttachmentPoint";
    }
    void Start()
    {
        InitComponents();
    }
    public AttachmentData GetData()
    {
        return attachedPoint.attachment.GetData();
    }
    void InitComponents()
    {
        attachment = transform.parent.GetComponent<Attachment>();
        gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<FixedJoint>();
        componentCollider = GetComponent<Collider>();
        componentRigidbody = GetComponent<Rigidbody>();
        componentJoint = GetComponent<FixedJoint>();

        if (componentCollider == null) Debug.LogError("AttachmentPoint must have a collider");
        if (componentRigidbody == null) Debug.LogError("AttachmentPoint must have a Rigidbody");
        componentRigidbody.useGravity = false;
        componentCollider.isTrigger = true;
        componentCollider.isTrigger = true;

        if (attachment.transform.GetComponent<Rigidbody>() == null) attachment.gameObject.AddComponent<Rigidbody>();
        componentJoint.connectedBody = attachment.transform.GetComponent<Rigidbody>();
    }
    void Attach(AttachmentPoint attachedPoint)
    {
        Debug.Log("attaching");
        this.attachedPoint = attachedPoint;
        attachedPoint.attachedPoint = this;
        Attachment attached = attachedPoint.attachment;
        Physics.IgnoreCollision(attached.GetComponent<Collider>(), attachment.GetComponent<Collider>(), true);
        AttachmentPoint toBeAttached, toAttachTo;
        if (attached.depth > attachment.depth)
        {
            toAttachTo = this;
            toBeAttached = attachedPoint;
        }
        else if (attached.depth < attachment.depth)
        {
            toAttachTo = attachedPoint;
            toBeAttached = this;
        }
        else
        {
            if (attachment.isHeld)
            {
                toAttachTo = this;
                toBeAttached = attachedPoint;
            }
            else
            {
                toAttachTo = attachedPoint;
                toBeAttached = this;
            }
        }
        toBeAttached.transform.SnapThisThenParent(toAttachTo.transform, () =>
        {
            toBeAttached.attachment.AttachTo(toAttachTo.attachment);
        });
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("AttachmentPoint") && Attachable(collision.GetComponent<AttachmentPoint>()))
        {
            Attach(collision.gameObject.GetComponent<AttachmentPoint>());
        }
    }
    bool Attachable(AttachmentPoint otherAttachedPoint)
    {
        if (attachedPoint != null) return false;
        Vector3 collisionPosition = otherAttachedPoint.transform.position;
        Vector3 collisionRotation = otherAttachedPoint.transform.eulerAngles;
        float positionDistance = Vector3.Distance(transform.position, collisionPosition);
        float rotationDistance = Vector3.Distance(transform.eulerAngles, collisionRotation);
        bool isPositionSnappable = positionDistance < snapDistance;
        bool isRotationSnappable = rotationDistance < snapRotation || rotationDistance > 360 - snapRotation;
        bool isTypeMatch = (type == AttachmentType.Hole && otherAttachedPoint.type == AttachmentType.Peg) || (type == AttachmentType.Peg && otherAttachedPoint.type == AttachmentType.Hole);
        Debug.Log(isPositionSnappable + ":" + positionDistance + " " + isRotationSnappable + ":" + rotationDistance + " " + isTypeMatch);
        return isPositionSnappable && isRotationSnappable && isTypeMatch;
    }
}

public enum AttachmentType // Peg goes into Hole
{
    Peg,
    Hole
}
public static class TransformExtensions
{
    // public static void SnapThisThenParent(this Transform current, Transform target, Action callback)
    // {
    //     if (current == null || target == null) return;

    //     Transform originalParent = current.parent;

    //     FixedJoint joint = current.GetComponent<FixedJoint>();
    //     // If there's no parent, just snap and return
    //     if (originalParent == null)
    //     {
    //         current.SetPositionAndRotation(target.position, target.rotation);
    //         return;
    //     }

    //     // Store original local transforms
    //     Vector3 originalLocalPosition = current.localPosition;
    //     Quaternion originalLocalRotation = current.localRotation;
    //     Debug.Log("before " + (originalParent.position - current.position));
    //     Debug.Log("before " + (originalLocalPosition));

    //     // Unparent and snap to target
    //     joint.connectedBody = null;
    //     current.SetParent(null);
    //     current.SetPositionAndRotation(target.position, target.rotation);
    //     Debug.Break();

    //     // Calculate new parent transform
    //     Quaternion newParentRotation = current.rotation * Quaternion.Inverse(originalLocalRotation);
    //     Vector3 newParentPosition = current.position + newParentRotation * originalLocalPosition;

    //     // Apply new transform to parent
    //     originalParent.SetPositionAndRotation(newParentPosition, newParentRotation);

    //     // Reparent while maintaining world position
    //     current.SetParent(originalParent);
    //     joint.connectedBody = originalParent.GetComponent<Rigidbody>();
    //     Debug.Log("after " + (originalParent.position - current.position));
    //     Debug.Log("after " + (current.localPosition));
    //     callback();
    // }
    public static void SnapThisThenParent(this Transform current, Transform target, Action callback)
    {
        if (current == null || target == null) return;

        Transform currentParent = current.parent;
        Transform currentGrandparent = currentParent.parent;
        FixedJoint joint = current.GetComponent<FixedJoint>();
        current.SetParent(null);
        joint.connectedBody = null;
        currentParent.SetParent(current);
        Vector3 parentLocalPosition = currentParent.localPosition;
        Quaternion parentLocalRotation = currentParent.localRotation;
        current.position = target.position;
        current.rotation = target.rotation;
        var sequence = DOTween.Sequence();
        // sequence.Append(currentParent.DOLocalMove(parentLocalPosition, 0.1f));
        // sequence.Join(currentParent.DOLocalRotate(parentLocalRotation.eulerAngles, 0.1f));
        // sequence.AppendCallback(() =>
        // {
        currentParent.localPosition = parentLocalPosition;
        currentParent.localRotation = parentLocalRotation;
        currentParent.SetParent(currentGrandparent);
        current.SetParent(currentParent);
        joint.connectedBody = currentParent.GetComponent<Rigidbody>();
        callback();
        // });
        // sequence.Play();
    }
    // public static void SnapThisThenParent(this Transform current, Transform target, Action callback)
    // {
    //     if (current == null || target == null || current.parent == null) return;
    //     Transform currentParent = current.parent;
    //     Vector3 offset = currentParent.position - current.position;
    //     Quaternion rotationOffset = Quaternion.Inverse(current.rotation) * currentParent.rotation;
    //     current.SetParent(null);
    //     FixedJoint joint = current.GetComponent<FixedJoint>();
    //     joint.connectedBody = null;
    //     Debug.Log(currentParent.position - current.position);
    //     var sequence = DOTween.Sequence();
    //     current.position = target.position;
    //     current.rotation = target.rotation;
    //     Debug.Log("snapping point");
    //     sequence.Append(currentParent.DOMove(current.position + offset, 0.1f));
    //     sequence.AppendCallback(() =>
    //     {
    //         currentParent.position = current.position + offset;
    //         currentParent.rotation = current.rotation * rotationOffset;
    //         current.SetParent(currentParent);
    //         Debug.Log(currentParent.position - current.position);
    //         joint.connectedBody = currentParent.GetComponent<Rigidbody>();
    //         callback();
    //     });
    //     sequence.Play();
    // }
    public static void InstantSnap(this Transform current, Transform target)
    {
        current.position = target.position;
        current.rotation = target.rotation;
        FixedJoint joint = current.GetComponent<FixedJoint>();
        Transform currentParent = current.parent;
        Quaternion rotationOffset = Quaternion.Inverse(current.rotation) * currentParent.rotation;
        Vector3 offset = currentParent.position - current.position;
        currentParent.position = current.position + offset;
        currentParent.rotation = current.rotation * rotationOffset;
        current.SetParent(currentParent);
        Debug.Log(currentParent.position - current.position);
        joint.connectedBody = currentParent.GetComponent<Rigidbody>();
    }
}

public class TransformSnapper : MonoBehaviour
{
}