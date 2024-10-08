using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Vector3 startPosition; // Starting position
    public Vector3 targetPosition; // Target position
    public float duration = 2f; // Duration to move from start to target

    private float elapsedTime = 0f; // Time elapsed since the movement started
    private bool movingToTarget = true; // Flag to track the current direction of movement

    void Start()
    {
        transform.position = startPosition; // Set the initial position
    }

    void Update()
    {
        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the percentage of completion
        float t = elapsedTime / duration;

        // Interpolate the position based on the current direction
        if (movingToTarget)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }
        else
        {
            transform.position = Vector3.Lerp(targetPosition, startPosition, t);
        }

        // Check if we've reached the target position
        if (t >= 1f)
        {
            movingToTarget = !movingToTarget; // Switch direction
            elapsedTime = 0f; // Reset elapsed time for the next movement
        }
    }
}
