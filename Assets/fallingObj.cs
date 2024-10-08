using UnityEngine;
using System.Collections;

public class SpawnAndShoot : MonoBehaviour
{
    public GameObject objectToSpawn; // The object to spawn
    public Transform player; // Reference to the player transform
    public float heightAbovePlayer = 2f; // Height above the player to spawn the object
    public float minXOffset = -2f; // Minimum random X offset
    public float maxXOffset = 2f; // Maximum random X offset
    public float shootForce = 10f; // Force applied to the object when shooting
    public float minSpawnTime = 1f; // Minimum time between spawns
    public float maxSpawnTime = 3f; // Maximum time between spawns

    void Start()
    {
        // Start the spawning coroutine
        StartCoroutine(SpawnObjectRoutine());
    }

    IEnumerator SpawnObjectRoutine()
    {
        while (true)
        {
            // Wait for a random amount of time
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // Spawn and shoot the object
            SpawnAndShootObject();
        }
    }

    void SpawnAndShootObject()
    {
        // Generate a random X offset
        float randomXOffset = Random.Range(minXOffset, maxXOffset);

        // Calculate the spawn position above and with the random offset from the player
        Vector3 spawnPosition = player.position + new Vector3(randomXOffset, heightAbovePlayer, 0f);

        // Instantiate the object
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // Get the Rigidbody component of the spawned object
        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Calculate the direction from the spawn position to the player's position
            Vector3 direction = (player.position - spawnPosition).normalized;

            // Apply force to shoot the object in the player's direction
            rb.AddForce(direction * shootForce, ForceMode.Impulse);
        }
    }
}

