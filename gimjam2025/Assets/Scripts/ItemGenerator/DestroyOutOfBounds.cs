using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < -2)
        {
            Destroy(gameObject);
            if (gameObject.transform.position.x > 0)
            {
                ItemGenerator.instance.conveyorTrackers.Find(x => x.name == "Left").items.RemoveAt(0);
            }
            else
            {
                ItemGenerator.instance.conveyorTrackers.Find(x => x.name == "Right").items.RemoveAt(0);
            }
        }
    }
}