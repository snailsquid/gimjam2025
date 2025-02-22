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
        Attachment attachment = other.GetComponent<Attachment>();
        if(hitTransform.CompareTag("Holdable"))
        {
            Debug.Log(gameObject);
            checking = attachment.EqualTo(attachmentPrefab);
            Submiting();
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        Transform hitTransform = other.gameObject.transform;
        if(hitTransform.CompareTag("Holdable"))
        {
            Debug.Log(gameObject);
            checking = false;
        }
    }
    void Submiting()
    {
        if(checking)
        {
            Debug.Log("You got the right thing");
            //Destroy();
        }
        else
        {
            Debug.Log("wrong");
            SceneManager.LoadScene("SubmitItem");
        }
    }
}
