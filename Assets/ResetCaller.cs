using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCaller : MonoBehaviour
{
    //public GameReset gameResetScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collided with Player!");
            other.GetComponent<characterScript>();
            characterScript characterScript = other.GetComponent<characterScript>();
            if (characterScript != null)
            {
                characterScript.playerDie();
                return;
            }
        }

    }

}
