using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    List<AttachmentPoint> attachmentPoints = new List<AttachmentPoint>();
    public Attachment pivotAttachment;
    public bool isHeld = false;
    Vector3 positionOffset;
    Vector3 rotationOffset;
    void Awake()
    {
        gameObject.AddComponent<Holdable>();
    }
    void Start()
    {
        InitPoints();
    }
    void Update()
    {
        if (pivotAttachment != null && pivotAttachment != this)
            Follow();
    }
    void Follow()
    {
        if (pivotAttachment == null) return;
        Transform pivotTransform = pivotAttachment.transform;
        transform.position = pivotTransform.position + positionOffset;
        transform.eulerAngles = pivotTransform.eulerAngles + rotationOffset;
    }
    public void SetOffsets()
    {
        positionOffset = transform.position - pivotAttachment.transform.position;
        rotationOffset = transform.eulerAngles - pivotAttachment.transform.eulerAngles;
    }
    public void AttachTo(Attachment attachment, AttachmentPoint previousAttachmentPoint = null)
    {
        Debug.Log("Attaching " + name + " to " + attachment.name + " with previous point " + previousAttachmentPoint);
        pivotAttachment = attachment;
        SetOffsets();
        AttachAttached(this, previousAttachmentPoint);
    }
    public void AttachAttached(Attachment attachedTo, AttachmentPoint previousAttachmentPoint) // Attach all attachment points to the attached object
    {
        foreach (AttachmentPoint attachmentPoint in attachmentPoints)
        {
            Debug.Log("Checking " + attachmentPoint.name + " on " + name);
            AttachmentPoint attachedPoint = attachmentPoint.attachedPoint;
            if (attachedPoint == null || attachmentPoint == previousAttachmentPoint) continue;
            attachedPoint.attachment.AttachTo(attachedTo, attachedPoint);
            SetOffsets();
        }
    }
    public void Detach()
    {
        pivotAttachment = null;
    }
    public void Hold()
    {
        isHeld = true;
    }
    public void Release()
    {
        isHeld = false;
    }
    void InitPoints()
    {
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("AttachmentPoint")) return;
            attachmentPoints.Add(child.GetComponent<AttachmentPoint>());
            Debug.Log("Added " + child.name + " to " + name);
        }
    }
}

