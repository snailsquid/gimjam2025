using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemGenerator;

public class ItemController : MonoBehaviour
{
    private readonly float speed = 1f;
    public float conveyorSpeed = 3f;
    private bool movingRight;
    public ItemManager.Direction direction;
    private readonly float targetX = 0.0f;
    private bool onConveyor = true;
    private Rigidbody rb;
    private List<ConveyorTracker> conveyorTrackers;
    public void Awake()
    {
        conveyorTrackers = FindObjectOfType<ItemGenerator>().conveyorTrackers;
    }

    void Start()
    {
        conveyorSpeed = ConveyorManager.Instance.initialSpeed;
        rb = GetComponent<Rigidbody>();
        transform.AddComponent<DestroyOutOfBounds>();
    }
    public void Initialize(ItemManager.Direction direction)
    {
        this.direction = direction;

        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    public void OnDestroy()
    {
        Debug.Log("Destroying " + gameObject.name);
        if (GetComponent<Attachment>() != null)
            ItemManager.Instance.AddToRespawnQueue(gameObject.name.Replace("(Clone)", ""));
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Conveyor"))
        {
            Debug.Log("Exiting conveyor");
            rb.velocity = new Vector3(rb.velocity.x, -speed, 0);
            onConveyor = false;
        }
    }
    private void FixedUpdate()
    {
        if (onConveyor)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
            float currentX = rb.position.x;
            foreach (ConveyorTracker conveyorTracker in conveyorTrackers)
            {
                if (direction == ItemManager.Direction.Left)
                {
                    if (currentX < targetX)
                    {
                        rb.velocity = Vector3.right * conveyorSpeed;
                    }
                }
                else
                {
                    if (currentX > targetX)
                    {
                        rb.velocity = Vector3.left * conveyorSpeed;
                    }
                }
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}