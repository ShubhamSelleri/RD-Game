using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gearController
{

    public class rotatingPlatform : MonoBehaviour, IMovablePlatform
    {
        public GameObject platform;                     // complete platform with everything included
        public GameObject ground;                       // only cube or whatever object
        public GameObject gearSet;                      // only gears
        public string rotationAxis = "z";               // axis of movement

        public float stepsPerRotation = 48f;            // steps needed to rotate platform once
        public float maxRotationClockwise = 90f;        // max rotation clockwise, -1f results in infinite
        public float maxRotationCounterClockwise = 0f;  // max rotation clockwise, -1f results in infinite

        public float[] gearRotationMultipliers;         // increase if gear needs to spin faster
        public string[] gearAxes;                       // rotation axis of each gear
        public bool isCoupled = false;                  // couples gears

        public bool isInverted = false;                 // inverts platform rotation

        private GameObject[] gears;                     // array of gears
        private float currentRotation = 0f;             // current platform position

        private float rotationStep = 0f;                // current rotationstep
        private GameObject playerOnPlatform;            // player on platform

        // Start is called before the first frame update
        void Start()
        {
            // make array of gears once
            gears = gearFunctions.getChildObjects(gearSet);
            //invert because of camera position
            isInverted = !isInverted;

        }

        void handleMessage(string msg)
        {
            //calcultate step but max rotations are swapped because of camera position
            rotationStep = gearFunctions.calculateStep(currentRotation, msg, 360f / stepsPerRotation,
                maxRotationClockwise, maxRotationCounterClockwise, isInverted);

            //move platform, otherwise do blockedAnimation
            if (rotationStep != 0f)
            {
                gearFunctions.rotateGear(platform, rotationStep, rotationAxis);

                //counter the rotation of the whole platform
                gearFunctions.rotateGear(gearSet, -rotationStep, "z");

                //do rotation of each gear
                gearFunctions.rotateGears(gears, rotationStep, gearAxes, gearRotationMultipliers, isCoupled, isInverted);

                currentRotation += rotationStep;
            }
            else
            {
                blockedAnimation();
            }
        }

        void blockedAnimation()
        {

        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerOnPlatform = other.gameObject;
                playerOnPlatform.transform.SetParent(transform);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerOnPlatform.transform.SetParent(null);
                playerOnPlatform = null;
            }
        }
    }

}
