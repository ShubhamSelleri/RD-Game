using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class GestureClient : MonoBehaviour
{
    // Define the Flask server URL
    private string url = "http://localhost:5000/predict";

    // Change the method to accept a Vector3 array
    public void SendGestureData(Vector3[] gestureData)
    {
        // Start the coroutine to send the data
        StartCoroutine(PostGestureData(gestureData));
    }

    // Coroutine to handle the HTTP POST request
    private IEnumerator PostGestureData(Vector3[] gestureData)
    {
        // Convert Vector3[] to a list of arrays (because JSON can't directly handle Vector3)
        List<float[]> gestureArrayData = new List<float[]>();
        foreach (Vector3 vector in gestureData)
        {
            gestureArrayData.Add(new float[] { vector.x, vector.y, vector.z });
        }

        // Manually serialize the gesture data into a JSON string
        string jsonData = SerializeGestureData(gestureArrayData);

        // Log the serialized JSON data to debug
        Debug.Log("Sending JSON data: " + jsonData);

        // Create a UnityWebRequest with the JSON data
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check for errors in the response
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // Parse the response
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    // Manually serialize the gesture data into a JSON string
    private string SerializeGestureData(List<float[]> gestureArrayData)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{\"gesture\":[");

        for (int i = 0; i < gestureArrayData.Count; i++)
        {
            float[] vector = gestureArrayData[i];
            sb.Append("[");
            sb.Append(vector[0].ToString("F2")).Append(",")
              .Append(vector[1].ToString("F2")).Append(",")
              .Append(vector[2].ToString("F2"));
            sb.Append("]");

            // Add comma between arrays, but not after the last one
            if (i < gestureArrayData.Count - 1)
            {
                sb.Append(",");
            }
        }

        sb.Append("]}");
        return sb.ToString();
    }
}
