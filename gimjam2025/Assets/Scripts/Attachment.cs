using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    [NonSerialized] public string uniqueKey;
    public List<AttachmentPoint> attachmentPoints = new List<AttachmentPoint>();
    public bool isHeld = false;
    public bool isHead = false;
    public int depth = 100000;
    public Hand hand;
    public string attachmentKey;
    Vector3 positionOffset;
    Rigidbody rigidBody;
    Quaternion rotationOffset;
    public AttachmentData GetData()
    {
        return new AttachmentData(this);
    }
    void Awake()
    {
        gameObject.AddComponent<Holdable>();
    }
    void Start()
    {
        uniqueKey = transform.name;
        InitPoints();
        rigidBody = GetComponent<Rigidbody>();
    }
    public void AttachTo(Attachment attachment, AttachmentPoint previousAttachmentPoint = null, int previousDepth = 0)
    {
        if (attachment.isHeld)
        {
            FixedJoint joint = gameObject.GetComponent<FixedJoint>();
            depth = previousDepth + 1;
            if (joint == null)
            {

                gameObject.AddComponent<FixedJoint>();
                joint = gameObject.GetComponent<FixedJoint>();
            }
            if (attachment == this)
            {
                Debug.Log("Setting " + name + " as parent");
                transform.SetParent(hand != null ? hand.transform : null);
                depth = 0;
                Destroy(joint);
            }
            else
            {
                Debug.Log("Attaching " + name + " to " + attachment.name + ", with previous point : " + previousAttachmentPoint);
                transform.SetParent(attachment.transform);
                Release();
                joint.connectedBody = attachment.GetComponent<Rigidbody>();
            }
            AttachAttached(attachment, previousAttachmentPoint);
        }
        else
        {
            Attachment held = FindHeld();
            held.isHeld = true;
            held.AttachTo(held);
        }
    }
    public Attachment FindHead(Attachment previous = null)
    {
        if (isHead) { Debug.Log(name + " is head"); return this; }
        foreach (AttachmentPoint attachmentPoint in attachmentPoints)
        {
            if (attachmentPoint.attachedPoint == null || attachmentPoint.attachedPoint.attachment == previous) continue;
            Debug.Log("searching " + name);
            Attachment attached = attachmentPoint.attachedPoint.attachment.FindHead(this);
            if (attached != null) return attached;
        }
        isHead = true;
        return this;
    }
    Attachment FindHeld(AttachmentPoint previousAttachmentPoint = null)
    {
        if (isHeld) { Debug.Log("self is held"); return this; }
        foreach (AttachmentPoint attachmentPoint in attachmentPoints)
        {
            if (attachmentPoint == previousAttachmentPoint || attachmentPoint.attachedPoint == null)
            {
                Debug.Log(attachmentPoint.attachedPoint + "Skipping");
                continue;
            }
            else if (attachmentPoint.attachedPoint.attachment.isHeld)
            {
                Debug.Log("Found held" + attachmentPoint.attachedPoint.attachment.name);
                return attachmentPoint.attachedPoint.attachment;
            }
            else
            {
                Debug.Log("Held not found, checking " + attachmentPoint.attachedPoint.attachment.name);
                return attachmentPoint.attachedPoint.attachment.FindHeld(attachmentPoint.attachedPoint);
            }
        }
        Debug.Log("There is no held, returning self");
        return this;
    }
    public void AttachAttached(Attachment attachedTo, AttachmentPoint previousAttachmentPoint) // Attach all attachment points to the attached object
    {
        foreach (AttachmentPoint attachmentPoint in attachmentPoints)
        {
            Debug.Log("Checking " + attachmentPoint.name + " on " + name);
            AttachmentPoint attachedPoint = attachmentPoint.attachedPoint;
            if (attachedPoint == null || attachmentPoint == previousAttachmentPoint) continue;
            attachedPoint.attachment.AttachTo(attachedTo, attachedPoint, depth);
        }
    }
    public void Hold(Hand hand)
    {
        if (this.hand == null)
        {
            this.hand = hand;
            isHeld = true;
            rigidBody.drag = 200;
            rigidBody.angularDrag = 200;
            AttachTo(this);
        }
    }
    public void Release()
    {
        isHeld = false;
        hand = null;
    }
    public void OutConveyor()
    {
    }
    void InitPoints()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("AttachmentPoint")) return;
            child.name = "AttachmentPoint" + count;
            attachmentPoints.Add(child.GetComponent<AttachmentPoint>());
            count++;
        }
    }
    public bool EqualTo(Attachment other)
    {
        if (this == null || other == null) return false;
        AttachmentData thisData = this.FindHead().GetData();
        AttachmentData otherData = other.FindHead().GetData();
        if (thisData == null || otherData == null) return false;
        return thisData.IsEqual(otherData);
    }
}

public class AttachmentData
{
    public string key;
    Attachment attachment;
    public AttachmentData(Attachment attachment)
    {
        this.attachment = attachment;
        key = attachment.uniqueKey;
    }
    public Dictionary<string, List<(string, string)>> GetDictionaryKeys(Dictionary<string, List<(string, string)>> previousKeys = null, string previous = null)
    {
        Debug.Log(key + ", previous : " + previous);
        Dictionary<string, List<(string, string)>> currentKeys = new Dictionary<string, List<(string, string)>>();
        if (previousKeys == null) previousKeys = new Dictionary<string, List<(string, string)>>();
        List<(string, string)> keys = new List<(string, string)>();
        foreach (KeyValuePair<string, List<(string, string)>> key in previousKeys) // Try adding previous keys to current keys
        {
            currentKeys.TryAdd(key.Key, key.Value);
        }
        foreach (AttachmentPoint attachmentPoint in attachment.attachmentPoints)
        {
            if (attachmentPoint.attachedPoint != null && attachmentPoint.attachedPoint.attachment.uniqueKey != previous)
            {
                Debug.Log(attachmentPoint.name + " of " + attachment + " is attached to " + attachmentPoint.attachedPoint.attachment.uniqueKey);
                keys.Add((attachmentPoint.name, attachmentPoint.attachedPoint.attachment.uniqueKey));
                currentKeys.TryAdd(key, keys);

                currentKeys = attachmentPoint.attachedPoint.attachment.GetData().GetDictionaryKeys(currentKeys, attachment.uniqueKey);
            }
            else
            {
                currentKeys.TryAdd(key, keys);
                continue;
            }
        }
        keys.Sort();
        currentKeys = currentKeys.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        return currentKeys;
    }
    public bool IsEqual(AttachmentData other)
    {
        Dictionary<string, List<(string, string)>> keys = GetDictionaryKeys();
        Dictionary<string, List<(string, string)>> otherKeys = other.GetDictionaryKeys();
        Debug.Log("{\n" + string.Join(",\n", otherKeys.Select(kv =>
            $"  \"{kv.Key}\": [\n    {string.Join(",\n    ", kv.Value.Select(v => $"{{ \"name\": \"{v.Item1}\", \"color\": \"{v.Item2}\" }}"))}\n  ]"
        )) + "\n}");
        Debug.Log("{\n" + string.Join(",\n", keys.Select(kv =>
            $"  \"{kv.Key}\": [\n    {string.Join(",\n    ", kv.Value.Select(v => $"{{ \"name\": \"{v.Item1}\", \"color\": \"{v.Item2}\" }}"))}\n  ]"
        )) + "\n}");
        return AreDictionariesEqual(keys, otherKeys);
    }
    static bool AreDictionariesEqual(Dictionary<string, List<(string, string)>> dict1, Dictionary<string, List<(string, string)>> dict2)
    {
        // Step 1: Check if both dictionaries have the same number of keys
        if (dict1.Count != dict2.Count)
            return false;

        // Step 2: Compare each key-value pair
        return dict1.All(kv =>
            dict2.TryGetValue(kv.Key, out var list) &&
            kv.Value.OrderBy(x => x).SequenceEqual(list.OrderBy(x => x)) // Compare lists ignoring order
        );
    }

}
