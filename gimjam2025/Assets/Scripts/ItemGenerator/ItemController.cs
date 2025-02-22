using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemGenerator;

public class ItemController : MonoBehaviour
{
    private readonly float speed = 1f;
    private bool movingRight;
    private readonly float targetX = 0.0f;
    private bool onConveyor = true;
    private Rigidbody rb;
    private List<ConveyorTracker> conveyorTrackers;
    public void Awake()
    {
        conveyorTrackers = FindObjectOfType<ItemGenerator>().conveyorTrackers;
    }

    public void Initialize(bool moveRight)
    {
        movingRight = moveRight;

        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    public void OnDestroy()
    {
        Debug.Log("Destroying " + gameObject.name);
        ItemManager.Instance.AddToRespawnQueue(gameObject.name.Replace("(Clone)", ""));
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Conveyor"))
        {
            onConveyor = false;
            rb.velocity = new Vector3(rb.velocity.x, -speed, 0);
        }
    }

    private void FixedUpdate()
    {
        if (onConveyor)
        {

            float currentX = rb.position.x;

            foreach (ConveyorTracker conveyorTracker in conveyorTrackers)
            {
                if (movingRight)
                {
                    if (currentX < targetX)
                    {
                        rb.AddForce(Vector3.right * speed);
                    }
                }
                else
                {
                    if (currentX > targetX)
                    {
                        rb.AddForce(Vector3.left * speed);
                    }
                }
            }
        }
    }
}