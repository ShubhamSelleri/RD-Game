using UnityEngine;

public class RotateOnClick : MonoBehaviour
{
    public float rotationAmount = 15f; // Amount to rotate in degrees

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            RotateCounterClockwise();
        }

        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            RotateClockwise();
        }
    }

    private void RotateCounterClockwise()
    {
        transform.Rotate(0, 0, rotationAmount);
    }

    private void RotateClockwise()
    {
        transform.Rotate(0, 0, -rotationAmount);
    }
}