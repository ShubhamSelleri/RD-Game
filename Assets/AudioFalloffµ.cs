using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFalloff : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera's Transform
    public float maxVolume = 1.0f;    // Maximum volume of the audio
    public float maxDistance = 10.0f; // Distance at which the audio becomes inaudible

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioFalloff: No AudioSource component found on this GameObject.");
        }

        if (cameraTransform == null)
        {
            Debug.LogError("AudioFalloff: Camera Transform is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        AdjustVolumeBasedOnDistance();
    }

    // Adjust the audio volume based on the distance from the camera
    void AdjustVolumeBasedOnDistance()
    {
        if (cameraTransform != null && audioSource != null)
        {
            float distance = Vector3.Distance(cameraTransform.position, transform.position);

            // Calculate volume as a linear interpolation between maxVolume and 0
            float volume = Mathf.Clamp01(1 - (distance / maxDistance)) * maxVolume;

            audioSource.volume = volume;
        }
    }
}
