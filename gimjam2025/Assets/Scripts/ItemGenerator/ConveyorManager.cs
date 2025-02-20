using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ConveyorSetup : MonoBehaviour
{
    public static ItemGenerator instance { get; private set; }
    public float conveyorWidth = 10f;
    public float conveyorHeight = 1f;
    private ItemGenerator itemGenerator;
    public GameObject conveyorPrefab;
    private Vector3 conveyorPosition = new Vector3(3, 0.75f, -7);

    void Awake()
    {
        itemGenerator = FindObjectOfType<ItemGenerator>();
    }
    void Start()
    {
        CreateConveyorSystem();
    }

    void CreateConveyorSystem()
    {
        // Create left conveyor
        Transform leftSpawnPoint = CreateConveyor(new Vector3(-conveyorPosition.x, conveyorPosition.y, conveyorPosition.z), "LeftConveyor", true);
        Transform rightSpawnPoint = CreateConveyor(new Vector3(conveyorPosition.x, conveyorPosition.y, conveyorPosition.z), "RightConveyor", false);

        itemGenerator.InitializeSpawnPoints(leftSpawnPoint, rightSpawnPoint);
    }

    Transform CreateConveyor(Vector3 position, string name, bool isLeft)
    {
        GameObject conveyor = Instantiate(conveyorPrefab);
        conveyor.name = name;
        conveyor.transform.position = position;

        // Create spawn point
        GameObject spawnPoint = new GameObject(name + "SpawnPoint");
        spawnPoint.transform.parent = conveyor.transform;
        spawnPoint.transform.position = new Vector3(
            isLeft ? position.x - 1 : position.x + 1,
            position.y + 0.5f,
            position.z
        );

        itemGenerator.spawnPoints = itemGenerator.spawnPoints.Concat(new[] { spawnPoint }).ToArray();

        return spawnPoint.transform;
    }
}