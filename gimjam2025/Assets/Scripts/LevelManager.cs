using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Level> levels;
    public static LevelManager instance { get; private set; }
    public List<GameObject> leftConveyor, rightConveyor;
    public int levelJson;
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

        if (GUILayout.Button("Generate JSON for item List"))
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
            string path = "Assets/Levels/" + levelManager.levelJson + ".json";
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
