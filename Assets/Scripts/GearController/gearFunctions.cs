using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gearController
{
    public static class gearFunctions
    {
        // returns step of position or rotation
        public static float calculateStep(float currentValue, string msg, float stepSize, float minValue = -1f, float maxValue = -1f, bool inverse = false)
        {
            msg = msg.Trim();
            bool isClockwise = msg == "1";

            if (inverse)
            {
                isClockwise = !isClockwise;
            }
            // calculate rotation and check if in boundries
            if (isClockwise && (maxValue == -1f || currentValue + stepSize <= maxValue))
            {
                return stepSize;
            }
            else
            if (!isClockwise && (minValue == -1f || currentValue - stepSize >= -minValue))
            {
                return -stepSize;
            }
            else
                return 0f;

        }
        // all input arrays need same length
        // rotates a set of gears
        // isCoupled inverts all even numbered gears

        public static void rotateGears(GameObject[] gears, float degrees, string[] rotationAxes, float[] rotationMultipliers, bool isCoupled = false, bool isInverted = false)
        {
            for (int i = 0; i < gears.Length; i++)
            {
                //inverts degrees if needed
                bool isGearInverted = (i % 2 == 0) ^ isInverted;
                degrees = isInverted ? -degrees : degrees;

                rotateGear(gears[i], degrees * rotationMultipliers[i], rotationAxes[i]);
            }
        }

        public static void rotateGear(GameObject gear, float degrees, string axis)
        {
            
            switch (axis)
            {
                case "x": // Rotate around X-axis
                    gear.transform.Rotate(degrees, 0f, 0f, Space.Self);
                    break;
                case "y": // Rotate around Y-axis
                    gear.transform.Rotate(0f, degrees, 0f, Space.Self);
                    break;
                case "z": // Rotate around Z-axis
                    gear.transform.Rotate(0f, 0f, degrees, Space.Self);
                    break;
                default:
                    Debug.LogWarning("Invalid rotation axis specified. Use 'x', 'y', or 'z'.");
                    break;
            }
        }

        public static void translateGear(GameObject gear, float step, string axis)
        {
            switch (axis)
            {
                case "x": // Rotate around X-axis
                    gear.transform.Translate(step, 0f, 0f, Space.Self);
                    break;
                case "y": // Rotate around Y-axis
                    gear.transform.Translate(0f, step, 0f, Space.Self);
                    break;
                case "z": // Rotate around Z-axis
                    gear.transform.Translate(0f, 0f, step, Space.Self);
                    break;
                default:
                    Debug.LogWarning("Invalid rotation axis specified. Use 'x', 'y', or 'z'.");
                    break;
            }

        }
    }
}


