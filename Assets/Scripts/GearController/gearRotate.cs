using UnityEngine;

public class gearRotate : MonoBehaviour
{
    // The target object we want to rotate (can be set in the Inspector)
    public GameObject targetObject;
    public string rotationAxis = "X";

    // Maximum value the rotary encoder can read
    private float maxEncoderValue = 60f;

    // Minimum and maximum rotation angles
    private float minRotation = 0f;  // Minimum angle (start position)
    private float maxRotation = 360f; // Maximum angle (full rotation)

    // This will hold the current angle
    private float currentRotation = 0f;
    private int lastPosition = 1;
    private int step = 0;

    // Invoked when a line of data is received from the serial device.
    void OnEnable()
    {
        
    }

    void OnDisable()
    {
    }

    void handleMessage(string msg)
    {
        // Parse the message to an integer (we expect values between 1 and 60)
        if (int.TryParse(msg, out int position))
        {
            step = position - lastPosition;
            Debug.Log("step = " + step + " , position = " + position);

            // Map the encoder value to a rotation angle (from 0 to 360 degrees)
            currentRotation = Mathf.Lerp(minRotation, maxRotation, ((step + maxEncoderValue) % maxEncoderValue) / maxEncoderValue);

            // Rotate the target object based on the mapped value
            if (targetObject != null)
            {
                switch (rotationAxis)
                {
                    case "x": // Rotate around X-axis
                        targetObject.transform.Rotate(currentRotation, 0f, 0f, Space.Self);
                        break;
                    case "y": // Rotate around Y-axis
                        targetObject.transform.Rotate(0f, currentRotation, 0f, Space.Self);
                        break;
                    case "z": // Rotate around Z-axis
                        targetObject.transform.Rotate(0f, 0f, currentRotation, Space.Self);
                        break;
                    default:
                        Debug.LogWarning("Invalid rotation axis specified. Use 'x', 'y', or 'z'.");
                        break;
                }
            }

            lastPosition = position;
        }
    }
}
