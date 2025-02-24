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
        AttachmentData.KeysListContainer levelKeys = GetLevelItem(ItemManager.Instance.level);
        return JsonUtility.ToJson(levelKeys) == JsonUtility.ToJson(levelKeys);
    }
}

[CustomEditor(typeof(LevelManager))]
[ExecuteInEditMode]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        LevelManager levelManager = (LevelManager)target;

        if (GUILayout.Button("Save Conveyor Belt"))
        {
            Debug.Log("creating JSON");
            Debug.Log(levelManager.rightConveyor.Count);
            ItemManager.ItemRawConveyor conveyorBelt = new ItemManager.ItemRawConveyor
            {
                left = new string[levelManager.leftConveyor.Count],
                right = new string[levelManager.rightConveyor.Count]
            };
            for (int i = 0; i < levelManager.leftConveyor.Count; i++)
            {
                Attachment attachment = levelManager.leftConveyor[i].GetComponent<Attachment>();
                conveyorBelt.left[i] = attachment.name;
            }
            for (int i = 0; i < levelManager.rightConveyor.Count; i++)
            {
                Attachment attachment = levelManager.rightConveyor[i].GetComponent<Attachment>();
                conveyorBelt.right[i] = attachment.name;
            }
            string json = JsonUtility.ToJson(conveyorBelt);
            string path = "Assets/Levels/" + levelManager.conveyorLevel + ".json";
            File.WriteAllText(path, json);
        }
        if (GUILayout.Button("Save Compound Item"))
        {
            string path = "Assets/Levels/" + levelManager.itemLevel + "_item.json";
            string json = JsonUtility.ToJson(levelManager.CompoundItem.GetData().GetKeys());
            File.WriteAllText(path, json);
        }
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
