using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class MouseLight : MonoBehaviour
{
    Wiimote mote;

    void Start(){
        WiimoteManager.FindWiimotes();
        mote = WiimoteManager.Wiimotes[0];
        mote.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL_EXT16);
        mote.Accel.CalibrateAccel(AccelCalibrationStep.A_BUTTON_UP);
        mote.SendPlayerLED(true, false, false, false);
    }

    void Update()
    {

        if(mote.Button.home){
            mote.Accel.CalibrateAccel(AccelCalibrationStep.A_BUTTON_UP);
        }
        
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;
        /*
        // Convert screen position to world position
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0; // Set z to 0 for 2D (if in 3D, adjust accordingly)

        // Set the position of the GameObject to the mouse position
        transform.position = mousePosition;
        */
        if(mote != null){
            float[] acell = mote.Accel.GetCalibratedAccelData();

            mousePosition.x = acell[0]- 0.3f;
            mousePosition.y = -acell[1] + 0.3f;

            if(mousePosition.x > -0.3f && mousePosition.x < 0.3f) mousePosition.x = 0;
            if(mousePosition.y > -0.3f && mousePosition.y < 0.3f) mousePosition.y = 0;
        }
    }
/*
    IEnumerator activateMote(){
        yield return new WaitUntil(() => WiimoteManager.HasWiimote());
        mote = WiimoteManager.Wiimotes[0];
    }*/
}
