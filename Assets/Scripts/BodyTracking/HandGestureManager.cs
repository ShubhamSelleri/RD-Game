using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class HandGestureManager : MonoBehaviour
{
    private string apiUrl = "http://127.0.0.1:5000/check_gesture";
    private bool isChecking = false;

    void Update()
    {
        if (!isChecking)
        {
            StartCoroutine(CheckHandGesture());
        }
    }

    IEnumerator CheckHandGesture()
    {
        isChecking = true;
        // UnityWebRequest request = UnityWebRequest.Post(apiUrl, "");
        UnityWebRequest request = UnityWebRequest.Post(apiUrl, new WWWForm());

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Response from Flask: " + jsonResponse);

            GestureResponse response = JsonUtility.FromJson<GestureResponse>(jsonResponse);
            if (response.status == "Gesture Matched")
            {
                TriggerGameAction(response.gesture);
            }
        }
        else
        {
            Debug.LogError("Error communicating with Flask server: " + request.error);
        }

        isChecking = false;
        yield return new WaitForSeconds(0.5f); // Adjust polling frequency
    }

    void TriggerGameAction(string gesture)
    {
        Debug.Log("Matched Gesture: " + gesture);

        switch (gesture)
        {
            case "hand1":
                Debug.Log("Action for Hand Gesture 1");
                // Trigger specific game action for "hand1"
                break;
            case "hand2":
                Debug.Log("Action for Hand Gesture 2");
                // Trigger specific game action for "hand2"
                break;
            default:
                Debug.Log("Unknown Gesture");
                break;
        }
    }

    [System.Serializable]
    public class GestureResponse
    {
        public string status;
        public string gesture;
    }
}