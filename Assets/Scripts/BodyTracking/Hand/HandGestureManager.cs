// using UnityEngine;
// using System.Collections;
// using UnityEngine.Networking;

// public class HandGestureManager : MonoBehaviour
// {
//     private string apiUrl = "http://127.0.0.1:5000/check_gesture";
//     private bool isChecking = false;

//     void Update()
//     {
//         if (!isChecking)
//         {
//             StartCoroutine(CheckHandGesture());
//         }
//     }

//     IEnumerator CheckHandGesture()
//     {
//         isChecking = true;

//         // Send a POST request to the Flask server
//         UnityWebRequest request = UnityWebRequest.Post(apiUrl, new WWWForm());
//         yield return request.SendWebRequest();

//         if (request.result == UnityWebRequest.Result.Success)
//         {
//             string jsonResponse = request.downloadHandler.text;
//             Debug.Log("Response from Flask: " + jsonResponse);

//             // Deserialize the response
//             GestureResponse response = JsonUtility.FromJson<GestureResponse>(jsonResponse);

//             // Handle different statuses
//             if (response.status == "Gesture Verified")
//             {
//                 TriggerGameAction(response.gesture);
//             }
//             else if (response.status == "Gesture Matched")
//             {
//                 Debug.Log($"Holding Gesture: {response.gesture} ({response.holding:F2} seconds)");
//             }
//             else
//             {
//                 Debug.Log("No Gesture Detected");
//             }
//         }
//         else
//         {
//             Debug.LogError("Error communicating with Flask server: " + request.error);
//         }

//         isChecking = false;

//         // Poll the server at a regular interval
//         yield return new WaitForSeconds(0.5f);
//     }

//     void TriggerGameAction(string gesture)
//     {
//         Debug.Log("Gesture Verified: " + gesture);

//         switch (gesture)
//         {
//             case "Thumbs Up":
//                 Debug.Log("Action for Thumbs Up");
//                 // Add game logic for "Thumbs Up" gesture
//                 break;
//             case "Thumbs Down":
//                 Debug.Log("Action for Thumbs Down");
//                 // Add game logic for "Thumbs Down" gesture
//                 break;
//             case "Middle Finger":
//                 Debug.Log("Action for Middle Finger");
//                 // Add game logic for "Middle Finger" gesture
//                 break;
//             default:
//                 Debug.Log("Unknown Gesture");
//                 break;
//         }
//     }

//     [System.Serializable]
//     public class GestureResponse
//     {
//         public string status;     // Status of the response (e.g., "Gesture Matched", "Gesture Verified", "No Gesture")
//         public string gesture;    // Name of the matched gesture (e.g., "Thumbs Up")
//         public float holding;     // Time the gesture has been held (optional, only for "Gesture Matched")
//     }
// }

using UnityEngine;
using System.Collections;
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

        // Send a POST request to the Flask server
        UnityWebRequest request = UnityWebRequest.Post(apiUrl, new WWWForm());
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 204)
            {
                // No state change
                Debug.Log("No State Change Detected");
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                GestureResponse response = JsonUtility.FromJson<GestureResponse>(jsonResponse);

                if (response.status == "Gesture Verified")
                {
                    TriggerGameAction(response.gesture);
                }
                else if (response.status == "No Gesture")
                {
                    Debug.Log("No Gesture Detected");
                }
            }
        }
        else
        {
            Debug.LogError("Error communicating with Flask server: " + request.error);
        }

        isChecking = false;
        yield return new WaitForSeconds(0.5f); // Polling interval
    }

    void TriggerGameAction(string gesture)
    {
        Debug.Log("Gesture Verified: " + gesture);

        switch (gesture)
        {
            case "Thumbs Up":
                Debug.Log("Action for Thumbs Up");
                // Add game logic for "Thumbs Up" gesture
                break;
            case "Thumbs Down":
                Debug.Log("Action for Thumbs Down");
                // Add game logic for "Thumbs Down" gesture
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

