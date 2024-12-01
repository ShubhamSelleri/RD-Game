using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gearController
{
    public class horizontalPlatform : MonoBehaviour
    {
        public GameObject platform; // complete platform with everything included
        public GameObject ground; // only cube or whatever object
        public GameObject gearSet; // only gears

        public float stepsPerUnit = 10f;
        public float maxUnitsRight = 5f;
        public float maxUnitsLeft = 5f;

        public float stepsPerRotation = 48f;
        public float[] gearRotationMultipliers;
        public string[] gearAxes;

        public bool isInverted;

        private GameObject[] gears;
        private float currentPosition = 0f;
        private float stepToRotationRatio = 0f;

        // Start is called before the first frame update
        void Start()
        {
            gears = gearFunctions.getChildObjects(gearSet);
            stepToRotationRatio = 1 / stepsPerUnit * stepsPerRotation * 360f;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void handleMessage(string msg)
        {
            float step = gearFunctions.calculateStep(currentPosition, msg, 1f / stepsPerUnit,
                    maxUnitsLeft, maxUnitsRight, isInverted);
            float rotation = step * stepToRotationRatio;
            Debug.Log("Step: " + step);
            if (step != 0f)
            {
                gearFunctions.translateGear(platform, step, "x"); //moves entire platform gears included
                gearFunctions.rotateGears(gears, rotation, gearAxes, gearRotationMultipliers, true, isInverted); //rotates gears
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

        public Vector3 getSpeed(float step)
        {
            return new Vector3(step / Time.deltaTime, 0, 0);
        }
    }
}
