using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public List<GameObject> platformGroup1; // Group 1: Platforms 1 and 3
    public List<GameObject> platformGroup2; // Group 2: Platforms 2 and 4
    private bool isGroup1Active = true; // Track which group is active

    void Start()
    {
        // Initialize platforms: Group 1 active, Group 2 inactive
        SetPlatformGroupActive(platformGroup1, true);
        SetPlatformGroupActive(platformGroup2, false);
    }

    // Method triggered by BroadcastMessage
    void handleMessageButton(string msg)
    {
        if (msg.Trim() == "p") // Button press triggers platform toggle
        {
            TogglePlatforms();
        }
    }

    private void TogglePlatforms()
    {
        // Toggle the active group
        isGroup1Active = !isGroup1Active;

        // Set visibility for both groups
        SetPlatformGroupActive(platformGroup1, isGroup1Active);
        SetPlatformGroupActive(platformGroup2, !isGroup1Active);

        Debug.Log($"Platforms Toggled: Group 1 Active: {isGroup1Active}, Group 2 Active: {!isGroup1Active}");
    }

    private void SetPlatformGroupActive(List<GameObject> platformGroup, bool isActive)
    {
        foreach (GameObject platform in platformGroup)
        {
            if (platform != null)
            {
                // // Enable/Disable Renderer and Collider
                // Renderer renderer = platform.GetComponent<Renderer>();
                // Collider collider = platform.GetComponent<Collider>();

                // if (renderer != null)
                //     renderer.enabled = isActive;

                // if (collider != null)
                //     collider.enabled = isActive;

                platform.SetActive(isActive); // Optionally disable GameObject entirely
                Debug.Log($"Platform {platform.name} set to: {isActive}");
            }
            else
            {
                Debug.LogWarning("Platform reference is missing!");
            }
        }
    }
}