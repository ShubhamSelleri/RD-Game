using UnityEngine;

public class HidePath : MonoBehaviour
{
    // This will be called when the script starts
    void Start()
    {
        // Loop through all child objects of the GameObject this script is attached to
        foreach (Transform child in transform)
        {

            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }
    }
}