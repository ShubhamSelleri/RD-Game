using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPickup : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with has the "Gem" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Gem"))
        {
            // Disable the object that we collided with
            collision.gameObject.SetActive(false);
        }
    }
}
