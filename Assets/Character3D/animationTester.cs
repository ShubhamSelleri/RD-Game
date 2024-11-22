using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationTester : MonoBehaviour
{
    public Animator animator;

    public bool falling {
        get {
            return falling;
        }
        set {
            falling = value;
            animator.SetBool("falling", falling);
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
