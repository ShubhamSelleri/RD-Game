using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gearController
{
    public class gearDoor : MonoBehaviour
    {
        public GameObject door;
        public string doorAxis = "y";

        public float doorMaxHeight = 5f; // doorHeight in units
        public float doorMinHeight = 0f; // door can only go up
        public float stepsPerUnit = 10f; 

        public float stepsPerRotation = 48f; //one to one with controller

        public GameObject gearSetLeft;
        public float[] gearRotationMultipliersLeft;
        public string[] gearAxesLeft;

        public GameObject gearSetRight;
        public float[] gearRotationMultipliersRight;
        public string[] gearAxesRight;

        private float doorPosition = 0f;
        private GameObject[] gearsLeft;
        private GameObject[] gearsRight;
        private float stepToRotationatio = 0f;

        // Start is called before the first frame update
        void Start()
        {
            //make arrays out of gearSet Objects
            gearsLeft = gearFunctions.getChildObjects(gearSetLeft);
            gearsRight = gearFunctions.getChildObjects(gearSetRight);

            stepToRotationatio = stepsPerUnit / stepsPerRotation * 360f;
        }

        // move door up or down and rotate gears correctly
        void handleMessage(string msg)
        {
            //step calculation and step to rotation conversion
            float doorStep = gearFunctions.calculateStep(doorPosition, msg, 1f / stepsPerUnit,
                doorMinHeight, doorMaxHeight, false);
            float rotation = doorStep * stepToRotationatio;

            doorPosition += doorStep;
            if (doorStep != 0f)
            {
                //door movement
                gearFunctions.translateGear(door, doorStep, doorAxis);

                //rotate gears that "move" door
                gearFunctions.rotateGears(gearsLeft, rotation, gearAxesLeft, gearRotationMultipliersLeft, true, false);
                gearFunctions.rotateGears(gearsRight, rotation, gearAxesRight, gearRotationMultipliersRight, true, true);
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
