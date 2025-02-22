using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class AttachmentPoint : MonoBehaviour
{
    [SerializeField] float snapDistance = 0.1f, snapRotation = 5f;
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

        if (attachment.transform.GetComponent<Rigidbody>() == null) attachment.gameObject.AddComponent<Rigidbody>();
        componentJoint.connectedBody = attachment.transform.GetComponent<Rigidbody>();
    }
    void Attach(AttachmentPoint attachedPoint)
    {
        this.attachedPoint = attachedPoint;
        attachedPoint.attachedPoint = this;
        Attachment attached = attachedPoint.attachment;
        Physics.IgnoreCollision(attached.GetComponent<Collider>(), attachment.GetComponent<Collider>(), true);
        attachedPoint.transform.SnapThisThenParent(transform, () =>
        {
            attached.AttachTo(attachment);
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
        bool isPositionSnappable = Vector3.Distance(transform.position, collisionPosition) < snapDistance;
        bool isRotationSnappable = Vector3.Distance(transform.eulerAngles, collisionRotation) < snapRotation;
        bool isTypeMatch = (type == AttachmentType.Hole && otherAttachedPoint.type == AttachmentType.Peg) || (type == AttachmentType.Peg && otherAttachedPoint.type == AttachmentType.Hole);
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
    public static void SnapThisThenParent(this Transform current, Transform target, Action callback)
    {
        if (current == null || target == null || current.parent == null) return;
        Transform currentParent = current.parent;
        Vector3 offset = current.position - currentParent.position;
        current.SetParent(null);
        var sequence = DOTween.Sequence();
        current.position = target.position;
        sequence.Append(currentParent.DOMove(target.position - offset, 0.1f));
        sequence.AppendCallback(() =>
        {
            currentParent.position = target.position - offset;
            current.SetParent(currentParent);
            callback();
        });
    }
}