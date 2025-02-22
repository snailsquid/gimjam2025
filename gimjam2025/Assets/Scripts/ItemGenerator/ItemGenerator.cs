using System.Collections.Generic;
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
        internal string name;
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
            name = "Left",
            spawnPoint = leftSpawn,
            maxItems = 2,
            itemSpacing = 1f
        });

        conveyorTrackers.Add(new ConveyorTracker
        {
            name = "Right",
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
        for (int i = 0; i < 2; i++)
        {
            ConveyorTracker conveyorTracker = conveyorTrackers[i];

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
                        SpawnItem(conveyorTracker);
                        return;
                    }
                }
            }
        }
    }
    private void SpawnItem(ConveyorTracker conveyorTracker)
    {
        Debug.Log("Spawning item");
        GameObject prefabToSpawn = ItemManager.Instance.GetNextItemPrefab();
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
        if (conveyorTracker.items.Count == 0)
        {
            newItem.tag = "Holdable";
        }
        else
        {
            conveyorTracker.items[0].tag = "Holdable";
        }

        // Add ItemController component
        ItemController itemController = newItem.AddComponent<ItemController>();
        bool isMovingRight = conveyorTracker.spawnPoint.position.x < 0;

        itemController.Initialize(isMovingRight);
    }
}