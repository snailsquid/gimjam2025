using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Submit : MonoBehaviour
{
    public Attachment attachmentPrefab;
    public bool checking;
    void Start()
    {
    }
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        Transform hitTransform = other.gameObject.transform;
        GameObject objects = other.gameObject;
        Attachment attachment = other.GetComponent<Attachment>();
        if (hitTransform.CompareTag("Holdable"))
        {
            Debug.Log(gameObject);
            checking = attachment.EqualTo(attachmentPrefab);
            Submiting(objects);

        }
    }
    void OnTriggerExit(Collider other)
    {
        Transform hitTransform = other.gameObject.transform;
        if (hitTransform.CompareTag("Holdable"))
        {
            Debug.Log(gameObject);
            checking = false;
        }
    }
    void Submiting(GameObject objects)
    {
        if (checking)
        {
            Debug.Log("You got the right thing");
        }
        else
        {
            Debug.Log("wrong");
            LevelManager.instance.Restart();
        }
    }
}
