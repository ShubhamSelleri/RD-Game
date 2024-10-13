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
    public LeapAudioManager audioManager;

    // Temp stuff
    public TMP_Text textMesh;
    private float timeElapsed = 0.0f; // Time elapsed in seconds
    public bool isRunning = true; // Is the timer running?
    public float frequency = 1.0f; // Frequency of the timer in seconds

    // Pose stuff
    public bool log_button = false; // Logs directions of the hand in directions.txt
    public bool button_get_gesture = false; // Gets gesture


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
            // Sound effects
            // Play ambient sound
            audioManager.UpdateAmbientSound();

            // Get hand speed combined x and y speed and normalize
            float handSpeed = Mathf.Abs(_leftHand.PalmVelocity.x) + Mathf.Abs(_leftHand.PalmVelocity.y);
            audioManager.UpdateMovementSound(handSpeed);

            //
            OnUpdateHand(_leftHand);
        }
        else {
            // Reset sounds
            audioManager.ResetSounds();

            //
            textMesh.SetText("No hand detected");
        }
    }

    private void HandleHandSound(Hand _hand)
    {
        // Play ambient sound
        audioManager.UpdateAmbientSound();

        // Get hand speed combined x and y speed and normalize
        float handSpeed = Mathf.Abs(_hand.PalmVelocity.x) + Mathf.Abs(_hand.PalmVelocity.y);
        audioManager.UpdateMovementSound(handSpeed);
    }

    // Pose stuff
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
        
        // Make request
        if (button_get_gesture || timeElapsed > frequency) {
            timeElapsed = 0.0f;
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
        if (log_button) {
            System.IO.File.AppendAllText("directions.txt", string.Join(", ", directions) + Environment.NewLine);
            log_button = false;
        }
    }
}
