using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ButtonBroadcast : MonoBehaviour
{
    public bool debugMode = false;

    private string methodName = "handleMessageButton";
    void OnMessageArrived(string msg)
    {
        BroadcastMessage(methodName, msg);
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
        {
            Debug.Log("Successfully connected to the button input device.");
        }
        else
        {
            Debug.Log("Failed to connect or disconnected from the button input device.");
        }
    }
    void Start()
    {
        Debug.Log("Debug mode of gearController: " + debugMode);
    }

    void Update()
    {
        if (debugMode)
        {
 
            if (Input.GetKeyDown(KeyCode.N))
            {
                BroadcastMessage(methodName, "0");
            }
            else
            if (Input.GetKeyDown(KeyCode.M))
            {
                BroadcastMessage(methodName, "1");
            }
        }
    }
}
