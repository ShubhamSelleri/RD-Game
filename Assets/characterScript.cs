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
    private bool isFalling;
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

    private bool isMovementPressed = false;
    private bool isGravityInvertedPressed = false;
    private bool isGravityInvertedPressedPrev = false;
    private bool isGravityInverted = false;
    private bool isCharacterInverted = false;
    private bool isJumpPressed = false;

    private bool isTopTouching;
    private bool isFootOnGround;

    private SphereCollider topCollider;

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

        playerInput.CharacterControls.InvertGravity.started += onGravityInverted;
        playerInput.CharacterControls.InvertGravity.canceled += onGravityInverted;

        setupJumpVariables();
        maxVerticalSpeed = Mathf.Max(initialJumpVelocity,maxVerticalSpeed);
    }

    void onGravityInverted(InputAction.CallbackContext context)
    {
        isGravityInvertedPressed = context.ReadValueAsButton();
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

        updateIsFootOnGround();
        handleGravityInversion();
        handleGravity();
        handleMaxVerticalSpeed();
        handleJump();

    }

    void updateIsFootOnGround()
    {
        if (isGravityInverted)
        {
            isFootOnGround = isTopTouching;
        }
        else
        {
            isFootOnGround = characterController.isGrounded;
        }
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void handleJump()
    {
        if (!isJumping && isFootOnGround && isJumpPressed)
        {
            animator.SetBool(JumpHash, true);
            isJumpAnimating = true;
            isJumping = true;
            if (isGravityInverted)
            {
                currentMovement.y = -initialJumpVelocity * 0.5f; //asumes initial y velocity is 0;
            }
            else
            {
                currentMovement.y = initialJumpVelocity * 0.5f; //asumes initial y velocity is 0;
            }
        }
        else if(!isJumpPressed && isJumping && isFootOnGround)
        {
            isJumping = false;
            animator.SetBool(JumpHash, false);
            isJumpAnimating = false;
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
    }

    void handleAnimation()
    {
        bool isAnimatorRunning = animator.GetBool(RunHash);
        //bool isAnimatorJumping = animator.GetBool(JumpHash);
        //bool isAnimatorFalling = animator.GetBool(FallingHash);
        Debug.Log("isMovementPressed: " + isMovementPressed);
        if (isMovementPressed && !isAnimatorRunning)
        {
            animator.SetBool(RunHash, true);
        }
        else if (!isMovementPressed && isAnimatorRunning) ;
        {
            animator.SetBool(RunHash, false);
        }
    }

    void handleGravity()
    {
        if (isFootOnGround)
        {
            if (isGravityInverted)
            {
                currentMovement.y = 0.05f;
            }
            else
            {
                currentMovement.y = -0.05f;
            }

            isFallingAnimating = false;
            animator.SetBool(FallingHash, false);
        }
        //checks Y velocity depending on gravity and if jump is released
        isFalling = (isGravityInverted ? currentMovement.y > 0.05f : currentMovement.y < -0.05f) || (!isJumpPressed && !isFootOnGround);

        if (isFalling && !isFallingAnimating)
        {
            animator.SetBool(JumpHash, false);
            isJumpAnimating = false;
            animator.SetBool(FallingHash, true);
            isFallingAnimating = true;
        }
        else if (!isFalling && isFallingAnimating)
        {
            animator.SetBool(FallingHash, false);
            isFallingAnimating = false;
        }

        if (isFalling)
        {
            animator.SetBool(FallingHash, true);
            animator.SetBool(JumpHash, false);
            isJumpAnimating = false;

            float previousYVelocity = currentMovement.y;
            float newYVelocity;
            float nextYVelocity;
            //change sign of gravity if needed

            if (isGravityInverted)
            {
                newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
                nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
                currentMovement.y = nextYVelocity;
            }
            else
            {
                newYVelocity = currentMovement.y - (gravity * fallMultiplier * Time.deltaTime);
                nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
                currentMovement.y = nextYVelocity;
            }
        }
        else
        {

            float previousYVelocity = currentMovement.y;
            float newYVelocity;
            float nextYVelocity;
            //change sign of gravity if needed
            if (isGravityInverted)
            {
                newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
                nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
                currentMovement.y = nextYVelocity;
            }
            else
            {
                newYVelocity = currentMovement.y - (gravity * Time.deltaTime);
                nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
                currentMovement.y = nextYVelocity;
            }
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

    void handleGravityInversion()
    {
        if(isGravityInvertedPressed && !isGravityInvertedPressedPrev && isFootOnGround)
        {
            
            transform.Rotate(Vector3.forward * 180f) ;
            isGravityInverted = !isGravityInverted;
            isCharacterInverted = !isCharacterInverted;
        }

        isGravityInvertedPressedPrev = isGravityInvertedPressed;
    }

    void handleRotation()
    {
        if (isMovementPressed)
        {
            if (currentMovementInput.x == 1)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 90, transform.eulerAngles.z);
            }
            else if (currentMovementInput.x == -1)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, -90, transform.eulerAngles.z);
            }
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

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is in the Ground layer
        if (other.gameObject.tag == "Ground")
        {
            isTopTouching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object is in the Ground layer
        if (other.gameObject.tag == "Ground")
        {
            isTopTouching = false;
        }
    }

}
