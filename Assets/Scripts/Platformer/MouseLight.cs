using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class MouseLight : MonoBehaviour
{
    private Wiimote wiimote;
    private IRData irData;
    public GameObject irDotPrefab; // Prefab for visualizing IR dots
    private GameObject[] irDots;   // Array to store instantiated IR dots
    private Vector3 vector3;

    void Start()
    {
        WiimoteManager.FindWiimotes(); // Find connected Wiimotes
        if (WiimoteManager.HasWiimote())
        {
            wiimote = WiimoteManager.Wiimotes[0];
            wiimote.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL); // Enable IR tracking
            wiimote.SendPlayerLED(true, false, false, true);
            wiimote.SetupIRCamera(IRDataType.BASIC); // Initialize IR tracking
            irData = wiimote.Ir; // Get IRData instance
            wiimote.Accel.CalibrateAccel(AccelCalibrationStep.A_BUTTON_UP);

            // Instantiate IR dot objects
            irDots = new GameObject[4];
            for (int i = 0; i < irDots.Length; i++)
            {
                irDots[i] = Instantiate(irDotPrefab);
                irDots[i].SetActive(false); // Hide initially
            }
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
        
        // Update wiimote state
        //WiimoteManager.ReadWiimoteData(); // Ensure data is being read

        // Read button presses
        if (wiimote.Button.a) {Debug.Log("A button pressed");}
        if (wiimote.Button.b) {Debug.Log("B button pressed");}

        // Check other buttons (like D-pad and trigger)
        if (wiimote.Button.d_up)
            Debug.Log("D-pad Up pressed");
        if (wiimote.Button.d_down)
            Debug.Log("D-pad Down pressed");
        if (wiimote.Button.plus)
            Debug.Log("Plus button pressed");

        // Read accelerometer data
        float[] accel = wiimote.Accel.GetCalibratedAccelData();
        Debug.Log($"Accel X: {accel[0]}, Y: {accel[1]}, Z: {accel[2]}");
        vector3 = new Vector3(accel[0], accel[1], 0);
        
        vector3[0] = (accel[0]- 0.3f)*20;
        vector3[1] = (-accel[1] + 0.3f)*20;

        if(vector3[0] > -0.3f && vector3[0] < 0.3f) vector3[0] = 0;
        if(vector3[1] > -0.3f && vector3[1] < 0.3f) vector3[1] = 0;

       
        transform.position = vector3;
         // Access IR data
        

        /* Loop through IR data points to visualize them
        for (int i = 0; i < 4; i++)
        {
            int x = irData.ir[i, 0];
            int y = irData.ir[i, 1];

            if (x != -1 && y != -1)
            {
                // Convert the IR coordinates to screen space
                Vector2 screenPos = new Vector2((float)x / 1023 * Screen.width, (float)y / 767 * Screen.height);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

                // Set IR dot position and activate it
                irDots[i].transform.position = worldPos;
                irDots[i].SetActive(true);
            }
            else
            {
                irDots[i].SetActive(false); // Hide if IR dot is not detected
            }
        }

        // Get and print IR midpoint and pointing position for debugging
        float[] midpoint = irData.GetIRMidpoint();
        float[] pointingPos = irData.GetPointingPosition();
        
        Debug.Log($"IR Midpoint: X: {midpoint[0]:F2}, Y: {midpoint[1]:F2}");
        Debug.Log($"Pointing Position: X: {pointingPos[0]:F2}, Y: {pointingPos[1]:F2}");
    */
    }

    void OnApplicationQuit()
    {
        if (wiimote != null)
        {
            WiimoteManager.Cleanup(wiimote);
            wiimote = null;
        }
    }
    /*
    void Start(){
        
        WiimoteManager.FindWiimotes();
        mote = WiimoteManager.Wiimotes[0];
        mote.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL_EXT16);
        mote.Accel.CalibrateAccel(AccelCalibrationStep.A_BUTTON_UP);
        mote.SendPlayerLED(true, false, false, true);
    }

    void Update()
    {
        //mote.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL_EXT16);

        if(mote.Button.a){
            mote.Accel.CalibrateAccel(AccelCalibrationStep.A_BUTTON_UP);
            mote.SendPlayerLED(false, true, true, false);
        }
        
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;
        
        // Convert screen position to world position
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0; // Set z to 0 for 2D (if in 3D, adjust accordingly)

        // Set the position of the GameObject to the mouse position
        
        
        //if(mote != null){
            float[] acell = mote.Accel.GetCalibratedAccelData();
            Debug.Log(acell[0].ToString());

            mousePosition[0] = acell[0]- 0.3f;
            mousePosition[1] = -acell[1] + 0.3f;

            if(mousePosition[0] > -0.3f && mousePosition[0] < 0.3f) mousePosition[0] = 0;
            if(mousePosition[1] > -0.3f && mousePosition[1] < 0.3f) mousePosition[1] = 0;
        //}
        transform.position = mousePosition;
    }

    */

}
