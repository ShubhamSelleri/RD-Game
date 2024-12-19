using System.Collections.Generic;
using UnityEngine;

public class ButtonWisp : MonoBehaviour
{
    public List<GameObject> disableGroup; // List of objects to disable
    public GameObject WispUITutorial;     // Separate field for the Wisp UI
    private bool isActionTriggered = false;

    void handleMessageButton(string msg)
    {
        if (!isActionTriggered && msg.Trim() == "p")
        {
            DisableObjects();
            DisableWispUI();
            isActionTriggered = true;
        }
    }

    private void DisableObjects()
    {
        foreach (GameObject obj in disableGroup)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"Disabled GameObject: {obj.name}");
            }
        }
    }

    private void DisableWispUI()
    {
        if (WispUITutorial != null)
        {
            WispUITutorial.SetActive(false);
            Debug.Log("Wisp UI Tutorial has been disabled.");
        }
    }
}
