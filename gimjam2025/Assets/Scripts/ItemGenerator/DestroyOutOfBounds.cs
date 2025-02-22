using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    
    private void Update()
    {
        if (transform.position.y < -2)
        {
            Debug.Log(ItemGenerator.instance.conveyorTrackers);
            if (gameObject.transform.position.x > 0)
            {
                ItemGenerator.instance.conveyorTrackers.Find(x => x.name == "Left").items.RemoveAt(0);
                Destroy(gameObject);
            }
            else
            {
                ItemGenerator.instance.conveyorTrackers.Find(x => x.name == "Right").items.RemoveAt(0);
                Destroy(gameObject);
            }
        }
    }
}