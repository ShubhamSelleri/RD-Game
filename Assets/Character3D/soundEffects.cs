using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffects : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioSource audioSource = animator.gameObject.GetComponent<AudioSource>();
        Debug.Log(audioSource);
        if (audioSource != null)
        {
            //audioSource.Stop();
            // Map animation state names to specific clips
            if (stateInfo.IsName("Jumping"))
            {
                audioSource.PlayOneShot( Resources.Load<AudioClip>("Audio/JumpSound"));
            }
            else if (stateInfo.IsName("Running"))
            {
                // Looped run
                audioSource.PlayOneShot( Resources.Load<AudioClip>("Audio/RunSoundCropped"));
            }
            else if (stateInfo.IsName("Landing"))
            {
                audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/JumpSound"));
            }
            else if (stateInfo.IsName("Falling")) {
                audioSource.Stop();
            }
            else
            {
                // Play nothing
            }
        }
    }
}

