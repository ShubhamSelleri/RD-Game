using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformToggle : MonoBehaviour
{
    // Start is called before the first frame updateprivate SpriteRenderer spriteRenderer;
    private Renderer cubeRenderer;  // To control visibility
    private Collider cubeCollider; // To control collision

    void Awake()
    {
        cubeRenderer = GetComponent<Renderer>();
        cubeCollider = GetComponent<Collider>();
    }

    public void SetActive(bool isActive)
    {
        // Toggle visibility and collision
        cubeRenderer.enabled = isActive;
        cubeCollider.enabled = isActive;
    }
}