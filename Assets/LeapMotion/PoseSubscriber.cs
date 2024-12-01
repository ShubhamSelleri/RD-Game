using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System;

public class PoseSubscriber : MonoBehaviour
{
    [SerializeField]
    private HandPoseDetector poseDetector; // Reference to your PoseDetector script or component

    public bool down = true;

    // 2 Events other scripts can subscribe to one for down pose and one for up
    public event Action OnPoseDown;
    public event Action OnPoseUp;

    

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
            OnPoseDown?.Invoke();
        }
        else {
            Debug.Log("Upwards!");
            OnPoseUp?.Invoke();
        }
    }
}

