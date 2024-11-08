using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class MouseLight : MonoBehaviour
{
    private Wiimote wiimote;

    void Start()
    {
        WiimoteManager.FindWiimotes(); // Find connected Wiimotes
        if (WiimoteManager.HasWiimote())
        {
            wiimote = WiimoteManager.Wiimotes[0];
            wiimote.SendPlayerLED(true, false, false, true);
        }
    }

    void Update()
    {
        if (wiimote == null) return;

        // Update wiimote state
        wiimote.SendDataReportMode(InputDataType.REPORT_BUTTONS);
        WiimoteManager.ReadWiimoteData(); // Ensure data is being read

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
        //float[] accel = wiimote.Accel.GetCalibratedAccelData();
        //Debug.Log($"Accel X: {accel[0]}, Y: {accel[1]}, Z: {accel[2]}");
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
