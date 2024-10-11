using UnityEngine;

public class ResetPositions : MonoBehaviour
{
    public Transform player; // The player's transform
    public Transform camera; // The camera's transform

    private Vector3 initialPlayerPosition;
    private Vector3 initialCameraPosition;

    void Start()
    {
        // Store the initial positions of the player and camera
        initialPlayerPosition = player.position;
        initialCameraPosition = camera.position;
    }

    void Update()
    {
        // Check if the player's Y position is below 0
        if (player.position.y < 0)
        {
            ResetPositionsToInitial();
        }
    }

    public void ResetPositionsToInitial()
    {
        // Reset player and camera positions to their initial positions
        player.position = initialPlayerPosition;
        camera.position = initialCameraPosition;

        // reset the player's velocity if using Rigidbody
        if (player.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
        }
    }
}