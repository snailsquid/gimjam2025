using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMove : MonoBehaviour
{
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Holdable")
        {
            collision.gameObject.transform.position += transform.forward * Time.deltaTime;
        }
    }
}
