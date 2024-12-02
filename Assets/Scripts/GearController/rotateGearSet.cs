using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gearController
{
    public class rotateGearSet : MonoBehaviour
    {
        public GameObject gearSet;                      // gearset

        public float stepsPerRotation = 48f;            // steps needed to rotate platform once
        public float maxRotationClockwise = -1f;        // max rotation clockwise, -1f results in infinite
        public float maxRotationCounterClockwise = -1f; // max rotation clockwise, -1f results in infinite

        public float[] gearRotationMultipliers;         // increase if gear needs to spin faster
        public string[] gearAxes;                       // rotation axis of each gear
        public bool isCoupled = false;                  // couples gears

        public bool isInverted = false;                 // inverts platform rotation

        private GameObject[] gears;                     // array of gears
        private float currentRotation = 0f;             // current platform position

        private float rotationStep = 0f;                // current rotationstep

        // Start is called before the first frame update
        void Start()
        {
            // make array of gears once
            gears = gearFunctions.getChildObjects(gearSet);
        }

        void handleMessage(string msg)
        {
            //calculate step
           rotationStep = gearFunctions.calculateStep(currentRotation, msg, 360f / stepsPerRotation,
                maxRotationCounterClockwise, maxRotationClockwise, isInverted);

            //update current rotation
            if (rotationStep != 0f )
            {
                currentRotation += rotationStep;
                gearFunctions.rotateGears(gears, rotationStep, gearAxes, gearRotationMultipliers, isCoupled, isInverted);
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
