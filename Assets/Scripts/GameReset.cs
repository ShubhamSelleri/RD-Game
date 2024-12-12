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
    public CharacterMovement PlayerMovementScript;

    public Transform cameraTransform;
    public Rigidbody playerRb;

    public float upperDeathY;
    public float lowerDeathY;
    public int gravity;

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
        //CameraScript.SetCameraPos(initCameraPos);
        if(PlayerMovementScript.velovityMultiplier==-1){
            PlayerMovementScript.InvertGravity();
        }
        playerRb.velocity = Vector3.zero;      // Remove linear velocity
        playerRb.angularVelocity = Vector3.zero; // Remove angular velocity
        StartCoroutine(respawnAnimation.AnimateAlpha());
        playerCharacterController.enabled=true;
    }
}
