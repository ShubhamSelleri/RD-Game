using UnityEngine;

public class gearRotate : MonoBehaviour
{
    // The target object we want to rotate (can be set in the Inspector)
    public GameObject targetObject;
    public string rotationAxis = "x";

    public int stepsPerRatation = 48; // 1 to 1 of actual gear controller
    public float maxRotationClockwise = -1f; // -1f results in infinite
    public float maxRotationCounterClockwise = -1f;

    // Minimum and maximum rotation per frame
    private float currentRotation = 0f;
    private float maxRotation = 360f;
    private float minRotation = 0f;

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
        //if (int.TryParse(msg, int ,bruh))
        {
            msg = msg.Trim();
            bool isClockwise = msg == "1"; // true if "1", false otherwise
            Debug.Log("msg: "+ msg + "  isClockwise: " +  isClockwise);
            // calculate rotation and check if in boundries
            if (isClockwise && (maxRotationClockwise == -1f || 1f / stepsPerRatation * 360f + currentRotation <= maxRotationClockwise))
            {
                currentRotation = Mathf.Lerp(minRotation, maxRotation, 1f / stepsPerRatation);
                //Debug.Log("clockwise rotation by " + currentRotation + " degrees");
            }

            else
            if (!isClockwise && (maxRotationCounterClockwise == -1f || 1f / stepsPerRatation * 360f - currentRotation >= -1f*maxRotationCounterClockwise))
            {
                currentRotation = Mathf.Lerp(minRotation, maxRotation, (stepsPerRatation -1f) / stepsPerRatation);
                Debug.Log("counterclockwise rotation by " + currentRotation + " degrees");
            }
            
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
        }
    }
}
