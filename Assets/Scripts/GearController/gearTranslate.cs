using UnityEngine;
namespace gearController
{
    public class gearTranslate : MonoBehaviour
    {
        // The target object we want to translate (can be set in the Inspector)
        public GameObject targetObject;
        public int stepsPerUnit = 48; // Steps per unit (1 unit = 1 meter)

        public float maxTranslationClockwise = -1f; // -1f means infinite movement
        public float maxTranslationCounterClockwise = -1f; // -1f means infinite movement

        public string translationAxis = "x"; // Specify the axis to translate: "x", "y", or "z"
        public bool isInverse = false;

        private float currentTranslation = 0f; // Tracks the current translation along the selected axis

        void handleMessage(string msg)
        {
            Debug.Log("CurrentTranslation: " + currentTranslation);
            //calculate step
            float step= gearFunctions.calculateStep(currentTranslation, msg, 1f / stepsPerUnit,
                maxTranslationCounterClockwise, maxTranslationClockwise, isInverse);

            //update current position
            currentTranslation += step;

            //do translation
            gearFunctions.translateGear(targetObject, step, translationAxis);
        }
    }
}
