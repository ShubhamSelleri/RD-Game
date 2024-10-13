using Leap;
using Leap.Attributes;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class LeapPoses : MonoBehaviour
{
    public LeapProvider leapProvider;

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

        Debug.Log("Hand Speed: " + handSpeed + " Hand Volume: " + movementSound.volume);
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
        // Here we can get additional information about the hand
    }
}
