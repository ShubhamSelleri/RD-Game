using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class PoseSubscriber : MonoBehaviour
{
    [SerializeField]
    private HandPoseDetector poseDetector; // Reference to your PoseDetector script or component

    public bool down = true;

    private void Start()
    {
        if (poseDetector != null)
        {
            // Subscribe to the "Pose Detected" event
            poseDetector.OnPoseDetected.AddListener(OnPoseDetectedHandler);

            // Subscribe to the "Pose Lost" event
            poseDetector.OnPoseLost.AddListener(OnPoseLostHandler);
        }
        else
        {
            Debug.LogError("PoseDetector reference is missing!");
        }
    }

    private void OnPoseLostHandler()
    {
        Debug.Log("Pose has been lost.");
    }

    private void OnPoseDetectedHandler()
    {
        if (down) {
            Debug.Log("Downwards!");
        }
        else {
            Debug.Log("Upwards!");
        }
    }
}

