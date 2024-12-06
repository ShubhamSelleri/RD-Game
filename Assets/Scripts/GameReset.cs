using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour
{
    public Vector3 initPlayerPos;
    public Vector3 initCameraPos;

    public ChangeAlpha respawnAnimation;

    public Transform playerTransform;
    public CharacterController playerCharacterController;

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
    public void ResetPositions()
    {
        playerCharacterController.enabled=false;
        playerTransform.position = initPlayerPos;
        CameraScript.SetCameraPos(initCameraPos);
        Physics.gravity = new Vector3(0, -9.81f, 0);
        playerRb.velocity = Vector3.zero;      // Remove linear velocity
        playerRb.angularVelocity = Vector3.zero; // Remove angular velocity
        StartCoroutine(respawnAnimation.AnimateAlpha());
        playerCharacterController.enabled=true;
    }
}
