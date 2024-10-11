using UnityEngine;
using System.Collections;
public class ToggleComponents : MonoBehaviour
{
    public float startDelay = 2.0f; // Delay before starting the toggling
    public float toggleInterval = 1.0f; // Time interval for toggling the components

    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    private void Start()
    {
        // Get the MeshRenderer and BoxCollider components
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        // Start the coroutine to toggle components
        StartCoroutine(ToggleComponentsCoroutine());
    }

    private IEnumerator ToggleComponentsCoroutine()
    {
        // Wait for the starting delay
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // Disable the components
            meshRenderer.enabled = false;
            boxCollider.enabled = false;

            // Wait for the toggle interval
            yield return new WaitForSeconds(toggleInterval);

            // Enable the components
            meshRenderer.enabled = true;
            boxCollider.enabled = true;

            // Wait for the toggle interval
            yield return new WaitForSeconds(toggleInterval);
        }
    }
}
