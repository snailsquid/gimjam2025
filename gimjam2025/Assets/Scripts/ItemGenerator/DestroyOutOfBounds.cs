using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{

    ItemController itemController;
    void Start()
    {
        itemController = GetComponent<ItemController>();
    }
    private void Update()
    {
        if (transform.position.y < -2)
        {
            Debug.Log(ItemGenerator.instance.conveyorTrackers);

            Debug.Log(itemController.direction);
            ItemGenerator.instance.conveyorTrackers.Find(x => x.direction == itemController.direction).items.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}