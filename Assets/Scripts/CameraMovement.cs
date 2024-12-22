using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraMovement : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public Vector2 gridSize = new Vector2(40f, 40f); // Size of the grid cells

    private Vector3 startingPosition; // The initial position of the camera
    public int gridYS;
    void Start()
    {
        // Initialize the starting position to the current camera position
        startingPosition = transform.position;
        gridYS = Mathf.RoundToInt((player.position.y - startingPosition.y) / gridSize.y);
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player transform is not assigned.");
            return;
        }

        // Calculate the player's position in grid units relative to the starting position
        int gridX = Mathf.RoundToInt((player.position.x - startingPosition.x) / gridSize.x);
        int gridY = Mathf.RoundToInt((player.position.y - startingPosition.y) / gridSize.y);

        // Compute the new camera position based on the grid coordinates
        Vector3 newCameraPosition = startingPosition;
        newCameraPosition.x = startingPosition.x + gridX * gridSize.x;
        newCameraPosition.y = startingPosition.y + gridY * gridSize.y;
        newCameraPosition.z = startingPosition.z; // Keep the original z-position
         if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            newCameraPosition.y = startingPosition.y + gridYS*gridSize.y;
        }

        // Set the camera's position
        transform.position = newCameraPosition;
    }
}