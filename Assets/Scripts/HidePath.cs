using UnityEngine;

public class HidePath : MonoBehaviour
{
    // This will be called when the script starts
    void Start()
    {
        // Loop through all child objects of the GameObject this script is attached to
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>(true);

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            // Disable each MeshRenderer
            meshRenderer.enabled = false;
        }
    }
}