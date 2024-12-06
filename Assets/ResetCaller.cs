using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCaller : MonoBehaviour
{
    public GameReset gameResetScript;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we collided with has the "Player" tag
        if (other.CompareTag("Player"))
        {
            gameResetScript.ResetPositions();
        }
    }

}
