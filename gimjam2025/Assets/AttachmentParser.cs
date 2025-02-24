using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentParser : MonoBehaviour
{
    [SerializeField] public Attachment attachment1, attachment2;
    [SerializeField] public bool areSame;
    public void CheckIfSame()
    {
        areSame = attachment1.EqualTo(attachment2);
    }
}
