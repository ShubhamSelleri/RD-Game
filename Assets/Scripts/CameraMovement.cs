using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public float snapDistancePositive = 5f; // Distance the player must move before snapping the camera
    public float snapDistanceNegative = 5f; // Distance the player must move before snapping the camera
    public float snapAmount = 1f; // Amount to snap the camera on the x-axis
    private bool right = false;

    private float lastPlayerXPosition;

    void Start()
    {
        if (player != null)
        {
            lastPlayerXPosition = player.position.x - 5; // Initialize last position
        }
    }

    void Update()
    {
            // Check how far the player has moved on the x-axis
            float distanceMoved = player.position.x - lastPlayerXPosition;
            if (right == false){
            // If the player has moved beyond the snap distance, snap the camera
            if (Mathf.Abs(distanceMoved) >= snapDistancePositive)
            {
                // Snap the camera
                SnapCameraPositive();

                // Update the last player position
                lastPlayerXPosition = player.position.x;
                right = false;
            }

            if (distanceMoved <= snapDistanceNegative)
            {
                // Snap the camera
                SnapCameraNegative();

                // Update the last player position
                lastPlayerXPosition = player.position.x;
                right = true;
            }
            }
            else{
                if (distanceMoved >= -snapDistanceNegative)
            {
                // Snap the camera
                SnapCameraPositive();

                // Update the last player position
                lastPlayerXPosition = player.position.x;
                right = false;
            }

            if (distanceMoved <= -snapDistancePositive)
            {
                // Snap the camera
                SnapCameraNegative();

                // Update the last player position
                lastPlayerXPosition = player.position.x;
                right = true;
            }
            }
    }

    private void SnapCameraPositive()
    {
        // Snap the camera position
        Vector3 newPosition = transform.position;
        newPosition.x += snapAmount; // Adjust the x position by the snap amount
        transform.position = newPosition;
    }

    private void SnapCameraNegative()
    {
        // Snap the camera position
        Vector3 newPosition = transform.position;
        newPosition.x = newPosition.x - snapAmount; // Adjust the x position by the snap amount
        transform.position = newPosition;
    }


    public void SetCameraPos(Vector3 pos){
            lastPlayerXPosition = player.position.x;
            transform.position=pos;
    }
}
