using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentLevel;
    public List<Level> levels;
    public static LevelManager instance { get; private set; }
    public List<GameObject> leftConveyor, rightConveyor;
    public int conveyorLevel;
    public Attachment CompoundItem;
    public int itemLevel;
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public AttachmentData.KeysListContainer GetLevelItem(int level)
    {
        string jsonFile = new StreamReader("Assets/Levels/" + level + "_item.json").ReadToEnd();
        AttachmentData.KeysListContainer keys = JsonUtility.FromJson<AttachmentData.KeysListContainer>(jsonFile);
        return keys;
    }
    public bool IsSameWithLevel(AttachmentData.KeysListContainer keysListContainer)
    {
        AttachmentData.KeysListContainer levelKeys = GetLevelItem(currentLevel);
        Debug.Log("checking if same these :");
        Debug.Log(JsonUtility.ToJson(keysListContainer));
        Debug.Log(JsonUtility.ToJson(levelKeys));
        bool same = JsonUtility.ToJson(keysListContainer) == JsonUtility.ToJson(levelKeys);
        Debug.Log("it is infact : " + same);
        return same;
    }
}



public class Item
{
    public string itemName;
    public Item(string itemName)
    {
        this.itemName = itemName;
    }
}
