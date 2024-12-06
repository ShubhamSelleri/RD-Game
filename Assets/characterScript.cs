using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;




public class characterScript : MonoBehaviour
{

    public float rotationFactorPerFrame = 15.0f;
    public float moveSpeed = 10f;
    public float maxVerticalSpeed = 10f;
    public float fallMultiplier = 2.0f;


    public float maxJumpHeight = 1.0f;
    public float maxJumpTime = 0.5f;
    private float initialJumpVelocity;

    private bool isJumping = false;
    private float gravity;

    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    private int RunHash;
    private int JumpHash;
    private int FallingHash;

    private bool isJumpAnimating;
    private bool isFallingAnimating;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;

    private bool isMovementPressed;
    private bool isGravityInverted = false;
    private bool isCharacterInverted = false;
    private bool isJumpPressed = false;


    private void Awake()
    {

        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();


        RunHash = Animator.StringToHash("Run");
        JumpHash = Animator.StringToHash("Jump");
        FallingHash = Animator.StringToHash("Falling");


        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;

        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        setupJumpVariables();
        maxVerticalSpeed = Mathf.Max(initialJumpVelocity,maxVerticalSpeed);
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        handleAnimation();
        handleRotation();

        characterController.Move(currentMovement * Time.deltaTime);
        handleGravityInversion();
        handleGravity();
        handleMaxVerticalSpeed();
        handleJump();

    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            animator.SetBool(JumpHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity * 0.5f; //asumes initial y velocity is 0;
        }
        else if(!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (2 * maxJumpHeight) / Mathf.Pow(timeToApex,2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;

        Debug.Log("timeToApex = " + timeToApex);
        Debug.Log("gravity = " + gravity);
        Debug.Log("initialJumpVelocity = " + initialJumpVelocity);
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * moveSpeed;
        isMovementPressed = currentMovementInput.x != 0;
        Debug.Log("currentMovement: " + currentMovement);
    }

    void handleAnimation()
    {
        bool isAnimatorRunning = animator.GetBool(RunHash);
        bool isAnimatorJumping = animator.GetBool(JumpHash);
        bool isAnimatorFalling = animator.GetBool(FallingHash);

        if (isMovementPressed && !isAnimatorRunning)
        {
            animator.SetBool(RunHash, true);
        }
        else if (!isMovementPressed && isAnimatorRunning)
        {
            animator.SetBool(RunHash, false);
        }
    }

    void handleMaxVerticalSpeed()
    {
        bool isMaxVerticalSpeedExceeded = Mathf.Abs(currentMovement.y) > maxVerticalSpeed;
        if (isMaxVerticalSpeedExceeded)
        {
            if (isGravityInverted)
            {
                currentMovement.y = maxVerticalSpeed;
            }
            else
            {
                currentMovement.y = -maxVerticalSpeed;
            }
        }
    }

    void handleGravity()
    {
        //checks Y velocity depending on gravity and if jump is released
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.05f;
            isFallingAnimating = false;
            animator.SetBool(FallingHash, false);
        }

        bool isFalling = currentMovement.y < -0.05f || (!isJumpPressed && !characterController.isGrounded);

        if(isFalling && !isFallingAnimating )
        {
            animator.SetBool(JumpHash,false);
            isJumpAnimating = false;
            animator.SetBool(FallingHash, true);
            isFallingAnimating = true;
        }
        else if(!isFalling && isFallingAnimating )
        {
            animator.SetBool(FallingHash, false);
            isFallingAnimating = false;
        }
        
        if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity;
            float nextYVelocity;
            //change sign of gravity if needed

            newYVelocity = currentMovement.y - (gravity * fallMultiplier * Time.deltaTime);
            nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity;
            float nextYVelocity;
            //change sign of gravity if needed
            newYVelocity = currentMovement.y - (gravity * Time.deltaTime);
            nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;
         
        }
    }

    void handleGravityInversion()
    {
        if(isGravityInverted && !isCharacterInverted)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            isCharacterInverted = true;
        }
        else if(!isGravityInverted && isCharacterInverted)
        {
            transform.rotation = Quaternion.Euler(10f, 180f, 0f);
            isCharacterInverted = false;
        }
    }

    void handleRotation()
    {
        Vector3 positionTolookAt = Vector3.zero;

        positionTolookAt.x = currentMovement.x;
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionTolookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

        }
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
