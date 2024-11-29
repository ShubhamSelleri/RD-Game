using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    // The target object we want to translate (can be set in the Inspector)
    public GameObject targetObject;
    public string translationAxis = "x"; // Specify the axis to translate: "x", "y", or "z"

    public int stepsPerUnit = 48; // Steps per unit (1 unit = 1 meter)
    public float maxTranslationClockwise = -1f; // -1f means infinite movement
    public float maxTranslationCounterClockwise = -1f; // -1f means infinite movement

    private Vector3 startingPosition;
    private float currentPosition = 0f; // Tracks the current translation along the selected axis

    void Start()
    {
        startingPosition = targetObject.transform.position;
    }

    void handleMessage(string msg)
    {
        // Determine direction based on message
        bool isClockwise = msg == "1"; // true if "1", false otherwise
        Debug.Log("msg: " + msg + "  isClockwise: " + isClockwise);

        // Calculate translation step
        float step = isClockwise ? 1f / stepsPerUnit : -1f / stepsPerUnit;

        // Check boundaries
        if (isClockwise && (maxTranslationClockwise == -1f || currentPosition + step <= maxTranslationClockwise))
        {
            currentPosition += step;
        }
        else if (!isClockwise && (maxTranslationCounterClockwise == -1f || currentPosition + step >= -maxTranslationCounterClockwise))
        {
            currentPosition += step;
        }
        else
        {
            Debug.LogWarning("Translation exceeds boundaries, movement blocked.");
            return; // Stop further processing
        }

        // Translate the object along the specified axis
        if (targetObject != null)
        {
            Vector3 translation = Vector3.zero; // Initialize translation vector

            // Set translation based on the selected axis
            switch (translationAxis.ToLower())
            {
                case "x":
                    translation = new Vector3(step, 0f, 0f);
                    break;
                case "y":
                    translation = new Vector3(0f, step, 0f);
                    break;
                case "z":
                    translation = new Vector3(0f, 0f, step);
                    break;
                default:
                    Debug.LogWarning("Invalid translation axis specified. Use 'x', 'y', or 'z'.");
                    return;
            }

            // Apply the translation
            targetObject.transform.Translate(translation, Space.World);
        }
        else
        {
            Debug.LogError("Target object is not assigned.");
        }
    }
}
