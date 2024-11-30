using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class bloom_radomizer : MonoBehaviour
{
    public PostProcessVolume volume;
    private float targetBloomIntensity = 0f;
    public float lerpSpeed = 25f;
    // Update is called once per frame
    void Update() {
        // Randomize bloom
        volume.profile.TryGetSettings(out Bloom bloom);

        // Lerp the intensity value over time
        bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, targetBloomIntensity, Time.deltaTime * lerpSpeed);

        // Check if the intensity has reached the target value
        if (Mathf.Approximately(bloom.intensity.value, targetBloomIntensity))
        {
            // Generate a new random value for the target intensity
            targetBloomIntensity = Random.Range(0f, 8f);
        }

    }
}
