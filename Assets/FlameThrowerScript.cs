using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerScript : MonoBehaviour
{
    public GameObject flames;
    public int offTime;
    public int onTime;
    public int startDelay;
    public AudioSource audioSource;
    public Transform cameraTransform; // Reference to the camera's transform
    public float maxVolume = 1.0f;    // Maximum volume of the audio
    public float maxDistance = 10.0f; // Distance at which the audio becomes inaudible

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ToggleObject());
    }

    // Update is called once per frame
    void Update()
    {
        AdjustVolumeBasedOnDistance();
    }

    // Coroutine to toggle the GameObject on and off
    IEnumerator ToggleObject()
    {
        yield return new WaitForSeconds(startDelay);
        while (true)
        {
            // Enable the object
            flames.SetActive(true);
            audioSource.PlayOneShot(audioSource.clip);

            // Wait for the "on time"
            yield return new WaitForSeconds(onTime);

            // Disable the object
            flames.SetActive(false);

            // Wait for the "off time"
            yield return new WaitForSeconds(offTime);
        }
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
