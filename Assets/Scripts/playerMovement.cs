using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public string groundLayer = "Ground"; 

    //private Rigidbody rb;
    private bool isGrounded;
    private Vector3 velocity;
    private CharacterController characterController;

    public LayerMask groundLayerMask;
    public Transform groundCheck;

    private float jumpStartTime;
    private float delayJump=0.5f;

    public int velovityMultiplier=1;

    public Animator animator;
    public float gravity=9.81f;
    public bool currGravity = true;

    // public PoseSubscriber poseSubscriberUp1;
    // public PoseSubscriber poseSubscriberUp2;
    // public PoseSubscriber poseSubscriberDown1;
    // public PoseSubscriber poseSubscriberDown2;
    public HandGestureManager handGestureManager;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();

        Physics.gravity = new Vector3(0, -gravity, 0);                                                                  
        Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
        InputSystem.PauseHaptics();

        // poseSubscriberDown1.OnPoseDown += upPoseHandler;
        // poseSubscriberUp1.OnPoseUp += downPoseHandler;
        // poseSubscriberDown2.OnPoseDown += upPoseHandler;
        // poseSubscriberUp2.OnPoseUp += downPoseHandler;

        // Subscribe to HandGestureManager events
        if (handGestureManager != null)
        {
            
            handGestureManager.onThumbsUp.AddListener(HandleThumbsUp);
            handGestureManager.onThumbsDown.AddListener(HandleThumbsDown);
        }
        else
        {
            Debug.LogWarning("HandGestureManager is not assigned in characterScript.");
        }
    }

    void Update()
    {
        isGrounded = characterController.isGrounded||Physics.CheckSphere(groundCheck.position, 0.2f, groundLayerMask);
        if (isGrounded) {
            animator.SetBool("Falling", false);
            StartCoroutine(vibrateController(0.2f, 0.15f, 0.7f));
        }
        else {
            animator.SetBool("Falling", true);
        }

        // If animator jump is true set to false
        if (animator.GetBool("Jump")) {
            animator.SetBool("Jump", false);
            // Also vibrate the xbox controller
            StartCoroutine(vibrateController(0.15f, 0.4f, 0.7f));
        }

        Move();
        Jump();

        if (Input.GetKeyDown(KeyCode.S))
        {
            InvertGravity();
        }
    }

    private void downPoseHandler() {
        if (currGravity) {
            InvertGravity();
            currGravity = false;
        }
    }

    private void upPoseHandler() {
        if (!currGravity) {
            InvertGravity();
            currGravity = true;
        }
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);
        movement.Normalize();
        // Rotate character to face movement direction
        if (movement.magnitude > 0.1f)
        {
            if(movement.x==1){
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 90, transform.eulerAngles.z);
            }
            else if(movement.x==-1){
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, -90, transform.eulerAngles.z);
            }
            animator.SetBool("Run", true);
        }
        else {
            animator.SetBool("Run", false);
        }

        // Apply movement
        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            animator.SetBool("Jump", true);
            jumpStartTime=Time.time;
        }
        if (isGrounded && Time.time>jumpStartTime+delayJump){
            velocity.y=0;
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velovityMultiplier*velocity * Time.deltaTime);
    }
    public void InvertGravity()
    {
        // Invert gravity
        velovityMultiplier = -velovityMultiplier;
        jumpStartTime=Time.time;
        transform.Rotate(0,0,180f);
        velocity.y=0;
    }
    
    IEnumerator vibrateController(float duration, float freq1, float freq2) {
        float startTime = Time.time;
        InputSystem.ResumeHaptics();
        while (Time.time < startTime + duration) {
            Gamepad.current.SetMotorSpeeds(freq1, freq2);
            yield return null;
        }
        InputSystem.PauseHaptics();
    }

    void HandleThumbsUp()
    {
        Debug.Log("Thumbs Up Gesture Detected: Keeping gravity normal.");
        if (currGravity) {
            InvertGravity();
            currGravity = false;
            Debug.Log("Thumbs Up Gesture Detected: Keeping gravity normal.");
            
        }
    }

    void HandleThumbsDown()
    {
        Debug.Log("Thumbs Down Gesture Detected: Keeping gravity normal.");
        if (!currGravity) {
            InvertGravity();
            currGravity = true;
            Debug.Log("Thumbs Down Gesture Detected: Keeping gravity normal.");
        }
    }

}
