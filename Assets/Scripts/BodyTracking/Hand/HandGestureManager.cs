using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Networking;

public class HandGestureManager : MonoBehaviour
{
    [System.Serializable] public class GestureEvent : UnityEvent { }

    public GestureEvent onThumbsUp;
    public GestureEvent onThumbsDown;

    // public CharacterMovement character;

    private string apiUrl = "http://127.0.0.1:5000/check_gesture";
    private float pollingInterval = 1f; // Time between requests
    private int maxRetries = 3; // Maximum retry attempts on failure

    private bool gravity = false;

    void Start()
    {
        StartCoroutine(PollServer());
    }

    IEnumerator PollServer()
    {
        while (true)
        {
            yield return CheckHandGesture();
            yield return new WaitForSeconds(pollingInterval);
        }
    }

    IEnumerator CheckHandGesture()
    {
        UnityWebRequest request = null;
        bool requestSucceeded = false;

        for (int retry = 0; retry < maxRetries; retry++)
        {
            request = UnityWebRequest.Post(apiUrl, new WWWForm());
            request.timeout = 5; // Set a timeout for the request
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                requestSucceeded = true;
                break;
            }
            else
            {
                Debug.LogWarning($"Retry {retry + 1}/{maxRetries}: Failed to connect to server. Error: {request.error}");
                yield return new WaitForSeconds(1f); // Wait before retrying
            }
        }

        if (!requestSucceeded)
        {
            Debug.LogError("Failed to communicate with the server after retries. Stopping gesture checks.");
            yield break; // Stop further execution if retries failed
        }

        ProcessResponse(request);
    }

    void ProcessResponse(UnityWebRequest request)
    {
        string jsonResponse = request.downloadHandler.text;

        try
        {
            GestureResponse response = JsonUtility.FromJson<GestureResponse>(jsonResponse);

            if (response.gesture == "No Gesture")
            {
                // Debug.Log("No Gesture Detected");
                return;
            }

            TriggerGameAction(response.gesture);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parsing JSON response: " + e.Message + "\nResponse: " + jsonResponse);
        }
    }

    void TriggerGameAction(string gesture)
    {
        // Debug.Log("Gesture Verified: " + gesture);

        switch (gesture)
        {
            case "Thumbs Up":
                // Debug.Log("Triggering Thumbs Up Event");
                onThumbsUp?.Invoke();
                // if (gravity == false){
                //     character.InvertGravity();
                //     gravity = true;
                // }
                break;

            case "Thumbs Down":
                // Debug.Log("Triggering Thumbs Down Event");
                onThumbsDown?.Invoke();
                // if (gravity == true){
                //     character.InvertGravity();
                //     gravity = false;
                // }
                break;

            case "Rock":
                Debug.Log("Action for Rock");
                // Add game logic for "Rock" gesture
                break;
            
            case "Middle Finger":
                Debug.Log("Action for Rock");
                // Add game logic for "Rock" gesture
                break;

            default:
                // Debug.LogWarning("Unknown Gesture: " + gesture);
                break;
        }
    }

    [System.Serializable]
    public class GestureResponse
    {
        public string gesture; // Name of the recognized gesture or "No Gesture"
    }
}
