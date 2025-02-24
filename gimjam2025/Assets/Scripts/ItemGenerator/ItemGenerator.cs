using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public class ConveyorTracker
    {
        public Transform spawnPoint;
        public List<GameObject> items = new List<GameObject>();
        public int maxItems = 2;
        public float itemSpacing = 1f;
        public ItemManager.Direction direction;
    }
    public List<ConveyorTracker> conveyorTrackers = new List<ConveyorTracker>();
    public static ItemGenerator instance { get; private set; }
    public GameObject[] spawnPoints;
    public GameObject[] itemPrefabs;
    private ItemManager itemManager;
    public float spawnInterval = 1f;
    private float timer;
    private bool isInitialized;

    public void InitializeSpawnPoints(Transform leftSpawn, Transform rightSpawn)
    {
        conveyorTrackers.Clear();

        conveyorTrackers.Add(new ConveyorTracker
        {
            direction = ItemManager.Direction.Left,
            spawnPoint = leftSpawn,
            maxItems = 2,
            itemSpacing = 1f
        });

        conveyorTrackers.Add(new ConveyorTracker
        {
            direction = ItemManager.Direction.Right,
            spawnPoint = rightSpawn,
            maxItems = 2,
            itemSpacing = 1f
        });

        isInitialized = true;
    }

    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (!isInitialized) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            TrySpawnItem();
            timer = 0f;
        }
    }

    private void TrySpawnItem()
    {
        foreach (ConveyorTracker conveyorTracker in conveyorTrackers)
        {
            {
                conveyorTracker.items.RemoveAll(item => item == null);
                if (conveyorTracker.items.Count < conveyorTracker.maxItems)
                {
                    bool canSpawn = true;

                    if (conveyorTracker.items.Count > 0 && conveyorTracker.items[conveyorTracker.items.Count - 1] != null)
                    {
                        float distanceFromLast = Mathf.Abs(
                            conveyorTracker.items[conveyorTracker.items.Count - 1].transform.position.x - conveyorTracker.spawnPoint.position.x
                        );

                        if (distanceFromLast < conveyorTracker.itemSpacing)
                        {
                            canSpawn = false;
                        }
                    }
                    if (canSpawn)
                    {
                        Debug.Log("can spawn");
                        ItemManager.ItemType item = ItemManager.Instance.GetNextItemPrefab(conveyorTracker.direction);
                        if (item == null) continue;
                        Debug.Log("Spawning item: " + item.prefab.name);
                        SpawnItem(conveyorTracker, item.prefab);
                        continue;
                    }
                    else Debug.Log("cant spawn");
                }
            }
        }
    }
    private void SpawnItem(ConveyorTracker conveyorTracker, GameObject prefabToSpawn)
    {
        Debug.Log("Spawning item");
        if (prefabToSpawn == null) return;

        // Spawn position with fixed height
        Vector3 spawnPosition = new Vector3(
            conveyorTracker.spawnPoint.position.x,
            conveyorTracker.spawnPoint.position.y,
            conveyorTracker.spawnPoint.position.z
        );

        GameObject newItem = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        conveyorTracker.items.Add(newItem);

        // Set tag to Holdable to the first item on the conveyor

        // Add ItemController component
        ItemController itemController = newItem.AddComponent<ItemController>();

        itemController.Initialize(conveyorTracker.direction);

        ItemManager.Instance.RemoveFromQueue(conveyorTracker.direction);
    }
}