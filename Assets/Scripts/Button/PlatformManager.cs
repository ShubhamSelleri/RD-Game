using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public List<PlatformToggle> platformGroup1; // Group 1: Platforms 1 and 3
    public List<PlatformToggle> platformGroup2; // Group 2: Platforms 2 and 4
    private bool isGroup1Active = true; // Track which group is active

    void Start()
    {
        // Initialize platforms: Group 1 active, Group 2 inactive
        SetPlatformGroupActive(platformGroup1, true);
        SetPlatformGroupActive(platformGroup2, false);
        // Debug.Log("Initialized: Group 1 is active, Group 2 is inactive.");
    }

    // Method triggered by BroadcastMessage
    void handleMessageButton(string msg)
    {
        // Debug.Log($"Received Message PlatfromManagerScript: {msg}");

        if (msg.Trim() == "p") // Button press triggers platform toggle   MAC version
        {
            TogglePlatforms();
        }
        // if (msg == "p") // Button press triggers platform toggle   WINDOWS version
        // {
        //     TogglePlatforms();
        // }
    }

    private void TogglePlatforms()
    {
        // Toggle the active group
        isGroup1Active = !isGroup1Active;

        // Set visibility for both groups
        SetPlatformGroupActive(platformGroup1, isGroup1Active);
        SetPlatformGroupActive(platformGroup2, !isGroup1Active);

        // Debug.Log($"Platforms Toggled: Group 1 Active: {isGroup1Active}, Group 2 Active: {!isGroup1Active}");
    }

    private void SetPlatformGroupActive(List<PlatformToggle> platformGroup, bool isActive)
    {
        foreach (PlatformToggle platform in platformGroup)
        {
            if (platform != null)
            {
                platform.SetActive(isActive);
                // Debug.Log($"Platform {platform.gameObject.name} set to: {isActive}");
            }
            else
            {
                Debug.LogWarning("PlatformToggle reference is missing!");
            }
        }
    }
}

