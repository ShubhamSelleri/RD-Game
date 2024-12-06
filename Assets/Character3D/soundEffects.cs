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
            // Map animation state names to specific clips
            if (stateInfo.IsName("Armature_001|Jump"))
            {
                audioSource.PlayOneShot( Resources.Load<AudioClip>("Audio/JumpSound"));
            }
            else if (stateInfo.IsName("Armature_001|Run"))
            {
                // Looped run
                audioSource.PlayOneShot( Resources.Load<AudioClip>("Audio/RunSound"));
            }
            else if (stateInfo.IsName("Armature_001|Landing"))
            {
                audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/JumpSound"));
            }
            else
            {
                // Play nothing
            }
        }
    }
}

