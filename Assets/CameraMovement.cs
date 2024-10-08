using UnityEngine;

public class CameraSegmentFollow : MonoBehaviour
{
    public Transform player; // The player's transform
    public float segmentWidth = 10f; // Width of each camera segment
    public float segmentHeight = 6f; // Height of each camera segment
    public float followSpeed = 0.1f; // Speed of camera movement
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position; // Initialize the target position
    }

    void Update()
    {
        if (player != null)
        {
            // Get the player's viewport position
            Vector3 playerViewportPos = Camera.main.WorldToViewportPoint(player.position);

            // Check if the player is near the left edge
            if (playerViewportPos.x < 0.1f)
            {
                // Move the camera left by one segment
                targetPosition.x -= segmentWidth;
            }
            // Check if the player is near the right edge
            else if (playerViewportPos.x > 0.9f)
            {
                // Move the camera right by one segment
                targetPosition.x += segmentWidth;
            }
        

            // Smoothly move the camera towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
        }
    }
}
