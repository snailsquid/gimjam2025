using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    List<AttachmentPoint> attachmentPoints = new List<AttachmentPoint>();
    void Start()
    {
        InitPoints();
        transform.tag = "Attachment";
    }
    void InitPoints()
    {
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("AttachmentPoint")) return;
            attachmentPoints.Add(child.gameObject.GetComponent<AttachmentPoint>());
        }
    }
}

