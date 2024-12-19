using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;
using System;

public class MouseLight : MonoBehaviour
{
    private Wiimote wiimote;
    private IRData irData;
    private Vector3 vector3;
    public float screenWidth = 16f;  // Width of Unity's world space
    public float screenHeight = 9f; // Height of Unity's world space
    public float movementSpeed = 15f; // Speed multiplier for movement

    public Transform camera;

    //public GameObject LaserPointer;
    //public GameObject FlashLight;
    private bool laserOn=true;


    void Start()
    {
        WiimoteManager.FindWiimotes(); // Find connected Wiimotes
        //FlashLight.SetActive(false);
        if (WiimoteManager.HasWiimote())
        {
            wiimote = WiimoteManager.Wiimotes[0];
            wiimote.SendDataReportMode(InputDataType.REPORT_EXT21); // Enable IR tracking
            wiimote.SendPlayerLED(true, false, false, true);
            wiimote.SetupIRCamera(IRDataType.BASIC); // Initialize IR tracking
            irData = wiimote.Ir; // Get IRData instance
            wiimote.Accel.CalibrateAccel(AccelCalibrationStep.A_BUTTON_UP);
        }
    }

    void Update()
    {
        if (wiimote == null) return;
        if (wiimote.ReadWiimoteData() == 0) return;

        if(wiimote.Button.a){
            wiimote.Accel.CalibrateAccel(AccelCalibrationStep.A_BUTTON_UP);
            wiimote.SendPlayerLED(false, true, true, false);
        }

        // Read button presses
        if (wiimote.Button.a) {Debug.Log("A button pressed");}
        if (wiimote.Button.b) {Debug.Log("B button pressed");}

        /*
        // Read accelerometer data
        float[] accel = wiimote.Accel.GetCalibratedAccelData();
        //Debug.Log($"Accel X: {accel[0]}, Y: {accel[1]}, Z: {accel[2]}");
        vector3 = new Vector3(accel[0], accel[1], 0);
        
        vector3[0] = (accel[0]- 0.3f)*20;
        vector3[1] = (-accel[1] + 0.3f)*20;

        if(vector3[0] > -0.3f && vector3[0] < 0.3f) vector3[0] = 0;
        if(vector3[1] > -0.3f && vector3[1] < 0.3f) vector3[1] = 0;

        vector3[0] = MathF.Round(vector3[0],1);
        vector3[1] = MathF.Round(vector3[1],1);
        transform.position = vector3;
        */
         IRData irData = wiimote.Ir;
        if (irData != null)
        {
            float[] pointingPosition = irData.GetPointingPosition();
            
            // Validate IR data
            if (pointingPosition[0] >= 0 && pointingPosition[1] >= 0)
            {
                // Map the pointing position (normalized 0-1) to Unity's world space
                float mappedX = Mathf.Lerp(-screenWidth*2, screenWidth*2, pointingPosition[0]);
                float mappedY = Mathf.Lerp(-screenHeight*2, screenHeight*2, pointingPosition[1]);

                // Move the block
                Vector3 targetPosition = new Vector3(mappedX + camera.position.x, mappedY, -5);
                transform.position = targetPosition;
                //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movementSpeed);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (wiimote != null)
        {
            WiimoteManager.Cleanup(wiimote);
            wiimote = null;
        }
    }
}
