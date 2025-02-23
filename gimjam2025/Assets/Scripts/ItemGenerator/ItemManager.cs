// ItemManager.cs - New script to manage item prefabs and spawning queue
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using System;
using System.Net.Mail;

public class ItemManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemRawConveyor
    {
        public string[] left, right;
    }
    public enum Direction
    {
        Left,
        Right
    }
    public class ItemType
    {
        public string itemName;
        public GameObject prefab;
        public Direction direction;
    }
    public int level = 1;
    public List<ItemType> availableItems;
    private LinkedList<ItemType> leftRespawn = new LinkedList<ItemType>();
    private LinkedList<ItemType> rightRespawn = new LinkedList<ItemType>();

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
    public void RemoveFromQueue(Direction direction)
    {
        LinkedList<ItemType> respawnQueue = direction == Direction.Left ? leftRespawn : rightRespawn;
        respawnQueue.RemoveFirst();
    }
    private void InitializeRespawnQueue(int level)
    {
        availableItems = new List<ItemType>();
        Debug.Log("playing level " + level);
        string jsonFile = new StreamReader("Assets/Levels/" + level.ToString() + ".json").ReadToEnd();
        ItemRawConveyor conveyorDirections = JsonUtility.FromJson<ItemRawConveyor>(jsonFile);
        Debug.Log(jsonFile);
        AddToConveyorQueue(leftRespawn, conveyorDirections.left, Direction.Left);
        AddToConveyorQueue(rightRespawn, conveyorDirections.right, Direction.Right);
        // foreach (KeyValuePair<string, List<ItemRaw>> conveyorDirection in conveyorDirections)
        // {
        //     Debug.Log(conveyorDirection.Key);
        //     Direction direction = conveyorDirection.Key == "Left" ? Direction.Left : Direction.Right;
        //     foreach (ItemRaw item in conveyorDirection.Value)
        //     {
        //         GameObject prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Items/" + item.itemName + ".prefab");
        //         ItemType itemType = new()
        //         {
        //             itemName = item.itemName,
        //             initialCount = item.initialCount,
        //             direction = direction,
        //             prefab = prefab,
        //         };
        //         availableItems.Add(itemType);
        //         if (direction == Direction.Left)
        //         {
        //             leftRespawn.AddFirst(itemType);
        //         }
        //         else
        //         {
        //             rightRespawn.AddFirst(itemType);
        //         }
        //     }
        // }
    }
    void AddToConveyorQueue(LinkedList<ItemType> respawnQueue, string[] items, Direction direction)
    {
        foreach (string item in items)
        {
            Debug.Log(item);
            ItemType itemType = new()
            {
                itemName = item,
                direction = direction,
                prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Items/" + item + ".prefab"),
            };
            availableItems.Add(itemType);
            respawnQueue.AddLast(itemType);
        }
    }

    public ItemType GetNextItemPrefab(Direction direction)
    {
        Debug.Log("Getting next item prefab for " + direction);
        LinkedList<ItemType> respawnQueue = direction == Direction.Left ? leftRespawn : rightRespawn;
        foreach (ItemType item in respawnQueue)
        {
            Debug.Log("in " + direction + " respawn there's " + item.itemName);
        }
        if (respawnQueue.Count > 0)
        {
            return respawnQueue.First();
        }
        return null;
    }

    public void AddToRespawnQueue(string itemName)
    {
        Debug.Log("Adding " + itemName + " to respawn queue");

        ItemType itemData = availableItems.Find(item => item.itemName == itemName);

        Debug.Log(itemData);
        ItemType newItem = new()
        {
            itemName = itemData.itemName,
            prefab = itemData.prefab,
            direction = itemData.direction,
        };

        LinkedList<ItemType> respawnQueue = itemData.direction == Direction.Left ? leftRespawn : rightRespawn;
        respawnQueue.AddFirst(newItem);
        foreach (ItemType item in respawnQueue)
        {
        }
    }
}