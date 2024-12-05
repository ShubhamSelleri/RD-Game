using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerPickup : MonoBehaviour
{
    private int gemsCollected=0;
    public TextMeshProUGUI TMPgemsCollected;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with has the "Gem" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Gem"))
        {
            // Disable the object that we collided with
            collision.gameObject.SetActive(false);
            gemsCollected+=1;
            TMPgemsCollected.text=gemsCollected.ToString();
        }
    }
}
