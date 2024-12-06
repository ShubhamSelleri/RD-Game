using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointActivation : MonoBehaviour
{
    public GameObject nonActiveCheckpoint;
    public GameObject activeCheckpoint;
    public GameReset gameResetScript;
    public Vector3 SpawnPos;
    public Transform cameraTransform;

    public Light worldLight;
    public float targetIntensity=1;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we collided with has the "Player" tag
        if (other.CompareTag("Player"))
        {
            nonActiveCheckpoint.SetActive(false);
            activeCheckpoint.SetActive(true);
            gameResetScript.initPlayerPos=SpawnPos;
            gameResetScript.initCameraPos=cameraTransform.position;
            worldLight.intensity = targetIntensity;
        }
    }
}
