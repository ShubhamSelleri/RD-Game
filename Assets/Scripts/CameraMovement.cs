using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public float snapDistance = 5f; // Distance the player must move before snapping the camera
    public float snapAmount = 1f; // Amount to snap the camera on the x-axis

    private float lastPlayerXPosition;

    void Start()
    {
        if (player != null)
        {
            lastPlayerXPosition = player.position.x; // Initialize last position
        }
    }

    void Update()
    {
            // Check how far the player has moved on the x-axis
            float distanceMoved = player.position.x - lastPlayerXPosition;

            // If the player has moved beyond the snap distance, snap the camera
            if (Mathf.Abs(distanceMoved) >= snapDistance)
            {
                // Snap the camera
                SnapCamera();

                // Update the last player position
                lastPlayerXPosition = player.position.x;
            }
    }

    private void SnapCamera()
    {
        // Snap the camera position
        Vector3 newPosition = transform.position;
        newPosition.x += snapAmount; // Adjust the x position by the snap amount
        transform.position = newPosition;
    }
    public void SetCameraPos(Vector3 pos){
            lastPlayerXPosition = player.position.x;
            transform.position=pos;
    }
}
