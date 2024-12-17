using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Cap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; // Cap the frame rate to 60 FPS
        Debug.Log("Frame rate capped to: " + Application.targetFrameRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
