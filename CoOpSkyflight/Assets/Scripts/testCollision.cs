using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Rigidbody>(out _))
        {
            print("[CollisionTest] Collided with " + collision.gameObject.name + "'s rigidbody.");
        }
    }
}
