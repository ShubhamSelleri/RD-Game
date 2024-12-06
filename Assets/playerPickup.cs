using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerPickup : MonoBehaviour
{
    private int gemsCollected=0;
    public TextMeshProUGUI TMPgemsCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            other.gameObject.SetActive(false);
            gemsCollected+=1;
            TMPgemsCollected.text=gemsCollected.ToString();
        }
    }
}
