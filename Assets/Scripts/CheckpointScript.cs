using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isActive = true; 
    private Vector3 respawnPoint;


    private void Start()
    {
        respawnPoint = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            // Try to get the CharacterScript from the player
            characterScript characterScript = other.GetComponent<characterScript>();
            if (characterScript != null)
            {
                // Call UpdateCheckpoint on the character
                characterScript.updateCheckpoint(respawnPoint);
                isActive = false;
            }
        }
    }

}