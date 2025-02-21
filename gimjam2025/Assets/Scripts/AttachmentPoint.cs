using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AttachmentPoint : MonoBehaviour
{
    [SerializeField] public AttachmentPointData data;
    [SerializeField] float snapDistance = 0.1f, snapRotation = 5f;
    [SerializeField] AttachmentType initialAttachmentType;
    public AttachmentPoint attachedPoint { get; private set; }
    Collider componentCollider;
    Rigidbody componentRigidbody;
    void Start()
    {
        transform.tag = "AttachmentPoint";
        data = new AttachmentPointData(transform.localPosition, transform.localEulerAngles, initialAttachmentType);
        componentCollider = GetComponent<Collider>();
        componentRigidbody = GetComponent<Rigidbody>();
        if (componentCollider == null) Debug.LogError("AttachmentPoint must have a collider");
        if (componentRigidbody == null) Debug.LogError("AttachmentPoint must have a Rigidbody");
        componentRigidbody.useGravity = false;
        componentCollider.isTrigger = true;
    }
    void Attach(AttachmentPoint attachedPoint)
    {
        this.attachedPoint = attachedPoint;
        if (attachedPoint.transform.parent.parent != null && attachedPoint.transform.parent.parent.CompareTag("CompositeItem"))
        {
            Debug.Log(transform);
            transform.parent.SetParent(attachedPoint.transform.parent.parent);
            transform.SnapThisThenParent(attachedPoint.transform);
        }
        else
        {
            GameObject parent = new GameObject();
            parent.tag = "CompositeItem";
            parent.name = "CompositeItem";
            Debug.Log(attachedPoint.transform.parent);
            Debug.Log(attachedPoint.transform);
            if (attachedPoint.transform.parent.parent != null)
                parent.transform.SetParent(attachedPoint.transform.parent.parent);
            transform.parent.SetParent(parent.transform);
        }
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
        Vector3 collisionPosition = otherAttachedPoint.transform.position;
        Vector3 collisionRotation = otherAttachedPoint.transform.eulerAngles;
        bool isPositionSnappable = Vector3.Distance(transform.position, collisionPosition) < snapDistance;
        bool isRotationSnappable = Vector3.Distance(transform.eulerAngles, collisionRotation) < snapRotation;
        bool isTypeMatch = (data.type == AttachmentType.Hole && otherAttachedPoint.data.type == AttachmentType.Peg) || (data.type == AttachmentType.Peg && otherAttachedPoint.data.type == AttachmentType.Hole);
        Debug.Log(isPositionSnappable.ToString() + " " + isRotationSnappable.ToString() + " " + isTypeMatch.ToString());
        return isPositionSnappable && isRotationSnappable && isTypeMatch;
    }
}

[Serializable]
public class AttachmentPointData
{
    [SerializeField] public Vector3 position, rotation; // Local 
    [SerializeField] public AttachmentType type;
    public AttachmentPointData(Vector3 position, Vector3 rotation, AttachmentType type)
    {
        this.position = position;
        this.rotation = rotation;
        this.type = type;
    }
}
public enum AttachmentType
{
    Peg,
    Hole
}
public static class TransformExtensions
{
    public static void SnapThisThenParent(this Transform current, Transform target)
    {
        if (current == null || target == null || current.parent == null) return;
        Transform currentParent = current.parent;
        Vector3 offset = current.position - currentParent.position;
        current.SetParent(null);
        var sequence = DOTween.Sequence();
        current.position = target.position;
        currentParent.position = target.position - offset;
        current.SetParent(currentParent);
    }
}