using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public LeapPoses leapPoses;
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to Event NewPose and link it to invertGravity()
        leapPoses.newPose += poseSetGravity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void poseSetGravity(string pose_string) {
        if (pose_string == "thumbs_up") {
            Physics.gravity = new Vector3(0, 9.81f, 0);
        }
        else if (pose_string == "thumbs_down") {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
        
    }
}
