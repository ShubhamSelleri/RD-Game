using UnityEngine;
namespace gearController
{
    public class gearRotate : MonoBehaviour
    {
        // The target object we want to rotate (can be set in the Inspector)
        public GameObject targetObject;
        public int stepsPerRotation = 48; // 1 to 1 of actual gear controller

        public float maxRotationClockwise = -1f; // -1f results in infinite
        public float maxRotationCounterClockwise = -1f;

        public string rotationAxis = "x";
        public bool inverse = false;

        // Minimum and maximum rotation per frame
        private float currentRotation = 0f;

        // Invoked when a line of data is received from the serial device.
        void OnEnable()
        {

        }

        void OnDisable()
        {

        }

        void handleMessage(string msg)
        {
            //calculate step
            float rotationStep = gearFunctions.calculateStep(currentRotation, msg, 360f / stepsPerRotation,
                maxRotationCounterClockwise, maxRotationClockwise, inverse);

            //update current rotation
            currentRotation += rotationStep;

            //do rotation
            gearFunctions.rotateGear(targetObject, rotationStep, rotationAxis);
        }
    }

}
