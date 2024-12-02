using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gearController
{
    public class movingPlatform : MonoBehaviour, IMovablePlatform
    {
        public GameObject platform;                 // complete platform with everything included
        public GameObject ground;                   // only cube or whatever object
        public GameObject gearSet;                  // only gears
        public string movementAxis = "x";           // axis of movement

        public float stepsPerUnit = 10f;            // steps needed to move platform 1 unit
        public float maxUnitsClockwise = 5f;        // right or up
        public float maxUnitsCounterClockwise = 5f; // left or down

        public float stepsPerRotation = 48f;        // steps needed to rotate gears once
        public float[] gearRotationMultipliers;     // increase if gear needs to spin faste
        public string[] gearAxes;                   // rotation axis of each gear
        public bool isCoupled = false;              // couples gears

        public bool isInverted;                     // inverts platform movement

        private GameObject[] gears;                 // array of gears
        private float currentPosition = 0f;         // current platform position
        private float stepToRotationRatio = 0f;     // amount of rotation of gears per step

        private float step = 0f;                    // current step
        private Vector3 velocity;                   // platform velocity
        private GameObject playerOnPlatform;        // player on platform

        // Start is called before the first frame update
        void Start()
        {
            // make array of gears once
            gears = gearFunctions.getChildObjects(gearSet);

            //calculate stepToRotationRatio once
            stepToRotationRatio = 1 / stepsPerUnit * stepsPerRotation * 360f;

            //initiate platformVelocity
            velocity = new Vector3(step, 0f, 0f);
        }


        void handleMessage(string msg)
        {
            //calcultate step
            step = gearFunctions.calculateStep(currentPosition, msg, 1f / stepsPerUnit,
                maxUnitsCounterClockwise, maxUnitsClockwise, isInverted);

            //move platform, otherwise do blockedAnimation
            if (step != 0f)
            {   
                
                float rotation = step * stepToRotationRatio;
                gearFunctions.translateGear(platform, step, movementAxis);
                gearFunctions.rotateGears(gears, rotation, gearAxes, gearRotationMultipliers, isCoupled, isInverted);

                currentPosition += step;
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
