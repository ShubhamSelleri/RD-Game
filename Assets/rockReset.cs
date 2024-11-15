using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockReset : MonoBehaviour
{
    public float yBotThreshold = -10f; // Y position threshold to reset the object
    public float yTopThreshold = 20f;
    public Vector3 botSpawnPosition;
    public GameObject rocks;
    private Vector3 initialPosition; // Store the initial position of the rock
    private Rigidbody rockRb; // Reference to the Rigidbody component
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = rocks.transform.position;
        rockRb=rocks.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rocks.transform.position.y < yBotThreshold)
        {
            ResetRockPosition(initialPosition);
        }
        else if(rocks.transform.position.y > yTopThreshold)
        {
            ResetRockPosition(botSpawnPosition);
        }
        
    }
    void ResetRockPosition(Vector3 spawnPos)
    {
        // Reset the position to the initial position
        if(spawnPos==botSpawnPosition){
            rocks.transform.rotation = Quaternion.Euler(180, 0, 0);
        }
        else{
            rocks.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        rocks.transform.position = spawnPos;
        rockRb.velocity = Vector3.zero;      // Remove linear velocity
        rockRb.angularVelocity = Vector3.zero; // Remove angular velocity
    }
}
