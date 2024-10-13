using Leap;
using Leap.Attributes;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LeapPoses : MonoBehaviour
{
    public LeapProvider leapProvider;
    //public GestureClient gestureClient;

    // Temp stuff
    public TMP_Text textMesh;
    private float timeElapsed = 0.0f; // Time elapsed in seconds
    public bool isRunning = true; // Is the timer running?
    public float frequency = 1.0f; // Frequency of the timer in seconds

    // Pose stuff
    public bool button = false;
    public bool button_get_gesture = false;


    // Sound properties
    public AudioSource ambientSound; // Ambient sound that plays when a hand is in the air
    public float volumeTransitionSpeed = 0.1f; // Speed at which the volume changes
    public float targetVolumeAmbient = 0.2f; // Target volume for ambient sound when the hand is detected

    public AudioSource movementSound; // Sound that plays when a hand is moving
    public float speedVolumeMultiplier = 1f; // Multiplier for movement sound based on hand speed
    public float targetVolumeMovement = 0.2f; // Target volume for movement sound based on speed

    private void OnEnable()
    {
        leapProvider.OnUpdateFrame += OnUpdateFrame;
    }

    private void OnDisable()
    {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame) {
        // Get the left hand from the frame
        Hand _leftHand = frame.GetHand(Chirality.Left);

        // If the hand is detected
        if (_leftHand != null) {
            HandleMovementSound(_leftHand);
            HandleAmbientSound(true); // Play ambient sound since hand is detected
            OnUpdateHand(_leftHand);
        }
        else {
            ResetSounds();
            textMesh.SetText("No hand detected");
        }
    }

    // Handles the movement sound volume based on hand speed
    private void HandleMovementSound(Hand _hand)
    {
        // Calculate the speed of the hand (absolute value to avoid negative speed)
        float handSpeed = Mathf.Abs(_hand.PalmVelocity.x);
        
        // Calculate the target volume based on hand speed
        float targetVolumeMovement = handSpeed * speedVolumeMultiplier;

        // Lerp the movement sound's volume towards the target volume
        movementSound.volume = Mathf.Lerp(movementSound.volume, targetVolumeMovement, Time.deltaTime * volumeTransitionSpeed);

        //Debug.Log("Hand Speed: " + handSpeed + " Hand Volume: " + movementSound.volume);
    }

    // Handles the ambient sound, either increasing or decreasing the volume based on hand presence
    private void HandleAmbientSound(bool isHandDetected)
    {
        if (ambientSound == null) return;

        if (isHandDetected) {
            // Lerp ambient sound volume upwards if hand is detected
            ambientSound.volume = Mathf.Lerp(ambientSound.volume, targetVolumeAmbient, Time.deltaTime * volumeTransitionSpeed);
        }
        else {
            // Lerp ambient sound volume downwards if no hand is detected
            ambientSound.volume = Mathf.Lerp(ambientSound.volume, 0.0f, Time.deltaTime * volumeTransitionSpeed);
        }
    }

    // Resets both the movement and ambient sounds when no hand is detected
    private void ResetSounds()
    {
        // Smoothly lower the movement sound volume to 0
        movementSound.volume = Mathf.Lerp(movementSound.volume, 0.0f, Time.deltaTime * volumeTransitionSpeed);
        
        // Lower ambient sound volume if needed
        HandleAmbientSound(false);
    }

    // Placeholder for additional hand information updates
    void OnUpdateHand(Hand _hand)
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime; // Increment elapsed time
        }

        // Here we can get additional information about the hand
        // Get all fingertip positions and rotations
        Finger [] fingers = _hand.fingers;

        // Loop with index
        // First create an array of positions and directions
        Vector3 [] directions = new Vector3[fingers.Length];

        for (int i = 0; i < fingers.Length; i++) {
            directions[i] = fingers[i].Direction;
        }

        // Log the positions and directions
        //Debug.Log("Directions: " + string.Join(", ", directions));
        
        // Make request
        if (button_get_gesture || timeElapsed > frequency) {
            timeElapsed = 0.0f;
            //gestureClient.SendGestureData(directions);
            // overwrite the gesture data in Assets/Scripts/Ultraleap/gesture_data.txt
            System.IO.File.WriteAllText("Assets/Scripts/Ultraleap/gesture_data.txt", string.Join(", ", directions));
            GestureAI.Inference();
            // Read predicted gesture from Assets/Scripts/Ultraleap/predicted_gesture.txt
            string predictedGesture = System.IO.File.ReadAllText("Assets/Scripts/Ultraleap/predicted_gesture.txt");
            Debug.Log("Predicted Gesture: " + predictedGesture);
            textMesh.SetText(predictedGesture);
            button_get_gesture = false;
        }


        // log directions to a file
        if (button) {
            System.IO.File.AppendAllText("directions.txt", string.Join(", ", directions) + Environment.NewLine);
            button = false;
        }
    }
}
