using Unity.VisualScripting;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private float speed = 5f;
    private bool movingRight;
    private float targetX = 0.0f;
    private const float STOP_THRESHOLD = 1.5f;
    private bool shouldMove = true;
    private Rigidbody rb;

    public void Initialize(bool moveRight)
    {
        movingRight = moveRight;

        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }

    // public void SetTargetPosition(float newTargetX)
    // {
    //     targetX = newTargetX;
    // }

    private void OnCollisionStay(Collision collision)
    {
        shouldMove = false;
        rb.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (!shouldMove) return;

        float currentX = rb.position.x;
        float distanceToTarget = Mathf.Abs(currentX - targetX);

        if (distanceToTarget > STOP_THRESHOLD)
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
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}