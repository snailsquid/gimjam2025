// ItemManager.cs - New script to manage item prefabs and spawning queue
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using System;

public class ItemManager : MonoBehaviour
{
    [System.Serializable]
    private class ItemRaw
    {
        public string itemName;
        public int initialCount;
    }
    private class ItemRaws { public ItemRaw[] itemRaws; }
    public class ItemType
    {
        public string itemName;
        public GameObject prefab;
        public int initialCount;
    }
    public int level = 1;
    public List<ItemType> availableItems;
    private LinkedList<ItemType> respawnQueue = new LinkedList<ItemType>();

    private static ItemManager instance;
    public static ItemManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeRespawnQueue(level);
    }

    private void InitializeRespawnQueue(int level)
    {
        availableItems = new List<ItemType>();
        string jsonFile = new StreamReader("Assets/Levels/" + level.ToString() + ".json").ReadToEnd();
        ItemRaws items = JsonUtility.FromJson<ItemRaws>(jsonFile);

        foreach (ItemRaw item in items.itemRaws)
        {
            ItemType itemType = new()
            {
                itemName = item.itemName,
                initialCount = item.initialCount,
                prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Items/" + item.itemName + ".prefab")
            };
            availableItems.Add(itemType);
            respawnQueue.AddFirst(itemType);
        }
    }

    public GameObject GetNextItemPrefab()
    {
        if (respawnQueue.Count > 0)
        {
            if (respawnQueue.First().initialCount <= 1)
            {
                ItemType nextItem = respawnQueue.First();
                respawnQueue.RemoveFirst();
                return nextItem.prefab;
            }
            else
            {
                respawnQueue.First().initialCount--;
                return respawnQueue.First().prefab;
            }
        }
        return null;
    }

    public void AddToRespawnQueue(string itemName)
    {
        Debug.Log("Adding " + itemName + " to respawn queue");
        
        ItemType itemData = availableItems.Find(item => item.itemName == itemName);

        ItemType newItem = new()
        {
            itemName = itemData.itemName,
            prefab = itemData.prefab,
            initialCount = 1
        };
        
        respawnQueue.AddFirst(newItem);
        foreach (ItemType item in respawnQueue)
        {
            Debug.Log(item.itemName + " " + item.initialCount);
        }
    }
}