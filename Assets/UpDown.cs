using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    // Amplitude of the oscillation (4 units up and down)
    public float amplitude = 4f;

    // Frequency of the oscillation (0.5 Hz)
    public float frequency = 0.5f;

    // Original position of the object
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the new Y position using a sine wave with a phase shift
        float newY = startPosition.y + amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.time + Mathf.PI / 2);

        // Update the object's position
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
