using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gearController
{
    public class gearDoor : MonoBehaviour
    {
        public GameObject door;
        public GameObject gearsLeft;
        public GameObject gearsRight;
        public float doorMaxHeight = 5f; // doorHeight in units
        public float doorMinHeight = 0f; // door can only go up
        public float stepsPerUnit = 10;
        public int[] gearRotationMultipliersLeft;
        public int[] gearRotationMultipliersRight;

        private float doorPosition = 0f;
        // Start is called before the first frame update


        // move door up or down and rotate gears correctly
        void handleMessage(string msg)
        {

            msg = msg.Trim();
            bool isMoveUp = msg == "1";
            

            //check if movement is valid otherwise do blocked animation
            if (isMoveUp && (doorPosition + 1 / stepsPerUnit <= doorMaxHeight))
            {
                //move door
                door.transform.Translate(Vector3.up * 1 / stepsPerUnit);
                doorPosition += 1 / stepsPerUnit;
                //rotate gears

            }
            else
            if (!isMoveUp && (doorPosition - 1 / stepsPerUnit >= doorMinHeight))
            {
                //move door
                door.transform.Translate(Vector3.down * 1 / stepsPerUnit);
                doorPosition -= 1 / stepsPerUnit;
                //rotate gears
            }
            else
            {
                blockedAnimation();
            }
        }
        void blockedAnimation()
        {

        }
    }
}
