using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour
{
    private Vector3 initPlayerPos;
    private Vector3 initCameraPos;

    public Transform playerTransform;

    public Transform cameraTransform;
    public Rigidbody playerRb;

    public float upperDeathY;
    public float lowerDeathY;

    public CameraMovement CameraScript;

    // Start is called before the first frame update
    void Start()
    {
        initCameraPos=cameraTransform.position;
        initPlayerPos=playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.y < lowerDeathY || playerTransform.position.y > upperDeathY)
        {
            ResetPositions();
        }
    }
    void ResetPositions()
    {
        playerTransform.position = initPlayerPos;
        CameraScript.SetCameraPos(initCameraPos);
        Physics.gravity = new Vector3(0, -9.81f, 0);
        playerRb.velocity = Vector3.zero;      // Remove linear velocity
        playerRb.angularVelocity = Vector3.zero; // Remove angular velocity
    }
}
