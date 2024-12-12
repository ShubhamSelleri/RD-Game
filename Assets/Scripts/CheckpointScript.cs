using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isActive = true; 
    private Vector3 respawnPoint;


    private void Start()
    {
        respawnPoint = transform.position + Vector3.up * 0.5f;
        Debug.Log("checkpoint position = "+ respawnPoint) ;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("sus");

        if (other.CompareTag("Player") && isActive)
        {
            characterScript characterScript = other.GetComponent<characterScript>();
            if (characterScript != null)
            {
                characterScript.updateCheckpoint(respawnPoint);
                Debug.Log("checkpoint activated");

                isActive = false;
            }
        }
    }

}