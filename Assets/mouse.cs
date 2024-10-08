using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert screen position to world position
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0; // Set z to 0 for 2D (if in 3D, adjust accordingly)

        // Set the position of the GameObject to the mouse position
        transform.position = mousePosition;
    }
}