using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapAudioManager : MonoBehaviour
{
    public AudioSource ambientSound;
    public AudioSource movementSound;
    public float ambientVolumeTransitionSpeed = 0.1f;
    public float movementVolumeTransitionSpeed = 0.1f;
    public float targetVolumeAmbient = 0.2f;
    public float speedVolumeMultiplier = 1f;

    public void UpdateMovementSound(float handSpeed)
    {
        float targetVolumeMovement = handSpeed * speedVolumeMultiplier;
        movementSound.volume = Mathf.Lerp(movementSound.volume, targetVolumeMovement, Time.deltaTime * movementVolumeTransitionSpeed);
    }

    public void UpdateAmbientSound()
    {
        ambientSound.volume = Mathf.Lerp(ambientSound.volume, targetVolumeAmbient, Time.deltaTime * ambientVolumeTransitionSpeed);
    }


    public void ResetSounds()
    {
        movementSound.volume = Mathf.Lerp(movementSound.volume, 0.0f, Time.deltaTime * movementVolumeTransitionSpeed);
        ambientSound.volume = Mathf.Lerp(ambientSound.volume, 0.0f, Time.deltaTime * ambientVolumeTransitionSpeed);
    }
}
