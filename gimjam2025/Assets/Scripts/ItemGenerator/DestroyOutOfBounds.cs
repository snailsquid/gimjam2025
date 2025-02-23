using System.Collections.Generic;
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
                List<GameObject> items = ItemGenerator.instance.conveyorTrackers.Find(x => x.direction == ItemManager.Direction.Left).items;
                Debug.Log("removing " + items[0]);
                items.RemoveAt(0);
                Destroy(gameObject);
            }
            else
            {
                ItemGenerator.instance.conveyorTrackers.Find(x => x.direction == ItemManager.Direction.Left).items.RemoveAt(0);
                Destroy(gameObject);
            }
        }
    }
}