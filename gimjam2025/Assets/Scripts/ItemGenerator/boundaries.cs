using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.x > 10 || transform.position.x < -10 ||
            transform.position.y > 2 || transform.position.y < -2)
        {
            Destroy(gameObject);
        }
    }
}