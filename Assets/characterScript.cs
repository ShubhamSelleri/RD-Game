using System.Collections.Generic;
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
    private float baseGravity;
    private float currentGravity;

    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    private int isRunningHash;
    private int isJumpingHash;
    private int isFallingHash;

    private bool isJumpAnimating;
    private bool isFallingAnimating;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;

    private bool isMovementPressed;
    private bool isGravityInverted = false;
    private bool isGravityInvertedPressedPrev;
    private bool isGravityInvertedPressed = false;
    private float gravityFloatingMultiplier = 1;
    private bool isJumpPressed = false;

    private bool isFootOnGround;
    private bool isHeadTouching = false;

    private Vector3 headPosition;
    private Vector3 feetPosition;
    private float characterRadius;
    private string groundLayer = "Ground";

    private SphereCollider headCollider;
    private HashSet<GameObject> groundObjects = new HashSet<GameObject>();

    private void Awake()
    {
        // get components
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // convert animator parameter strings to hash for performance
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isFallingHash = Animator.StringToHash("isFalling");

        // bind listen to inputs
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;

        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        playerInput.CharacterControls.InvertGravity.started += onInvertGravity;
        playerInput.CharacterControls.InvertGravity.canceled += onInvertGravity;

        setupJumpVariables();

        // set maxVertical speed to the highest minimum value needed to make jump work
        maxVerticalSpeed = Mathf.Max(initialJumpVelocity,maxVerticalSpeed);

        setupIsGrounded();
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

        handleIsGrounded();
        handleGravityInversion();
        handleGravity();
        handleMaxVerticalSpeed();
        handleJump();

    }

    void handleJump()
    {
        if (!isJumping && isFootOnGround && isJumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            if (!isGravityInverted)
            {
                currentMovement.y = initialJumpVelocity * 0.5f; //asumes initial y velocity is 0;
            }
            else
            {
                currentMovement.y = -initialJumpVelocity * 0.5f; //asumes initial y velocity is 0;
            }
        }
        else if(isJumping && isFootOnGround && !isJumpPressed)
        {
            animator.SetBool(isJumpingHash, false);
            isJumpAnimating = false;
            isJumping = false;

        }
    }

    void handleAnimation()
    {
        bool isAnimatorRunning = animator.GetBool(isRunningHash);
        bool isAnimatorJumping = animator.GetBool(isJumpingHash);
        bool isAnimatorFalling = animator.GetBool(isFallingHash);

        if (isMovementPressed && !isAnimatorRunning && !isJumping)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if (!isMovementPressed && isAnimatorRunning)
        {
            animator.SetBool(isRunningHash, false);
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
        float groundedGravity = 0.05f;
        bool isFalling;

        if (isFootOnGround)
        {
            currentMovement.y = isGravityInverted ? groundedGravity : -groundedGravity;
            animator.SetBool(isJumpingHash, false);
            isJumpAnimating = false;
        }
        //checks Y velocity depending on gravity and if jump is released
        isFalling = (isGravityInverted ? currentMovement.y > groundedGravity: currentMovement.y < -groundedGravity) || (!isJumpPressed && !isFootOnGround);

        if(isFalling && !isFallingAnimating)
        {
            animator.SetBool(isFallingHash, true);
            isFallingAnimating = true;
        }
        else if(!isFalling && isFallingAnimating)
        {
            animator.SetBool(isFallingHash, false);
            isFallingAnimating = false;
        }
        
        if (isFalling)
        {
            animator.SetBool(isJumpingHash, false);
            isJumpAnimating = false;

            float previousYVelocity = currentMovement.y;
            float newYVelocity;
            float nextYVelocity;
            //change sign of gravity if needed
            if (isGravityInverted)
            {
                newYVelocity = currentMovement.y + (currentGravity * fallMultiplier* Time.deltaTime);
                nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
                currentMovement.y = nextYVelocity;
            }
            else
            {
                newYVelocity = currentMovement.y - (currentGravity * fallMultiplier * Time.deltaTime);
                nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
                currentMovement.y = nextYVelocity;
            }
        }
        //else the character is still jumping "up"
        else
        {

            float previousYVelocity = currentMovement.y;
            float newYVelocity;
            float nextYVelocity;
            //change sign of gravity if needed
            if (isGravityInverted)
            {
                newYVelocity = currentMovement.y + (currentGravity * Time.deltaTime);
                nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
                currentMovement.y = nextYVelocity;
            }
            else
            {
                newYVelocity = currentMovement.y - (currentGravity * Time.deltaTime);
                nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
                currentMovement.y = nextYVelocity;
            }
        }
    }

    void handleIsGrounded()
    {
        Vector3 headDetectionCenter = transform.position + headPosition;
        headDetectionCenter.y +=  -0.05f;

        Vector3 feetDetectionCenter = transform.position + feetPosition;
        feetDetectionCenter.y +=  0.05f;

        isFootOnGround = Physics.CheckSphere(feetDetectionCenter,0.1f, LayerMask.GetMask(groundLayer));
        isHeadTouching = Physics.CheckSphere(headDetectionCenter, 0.1f , LayerMask.GetMask(groundLayer));
        Debug.Log("isFootOnGround = " + isFootOnGround);
        Debug.Log("isHeadTouching = " + isHeadTouching);
    }

    void handleGravityInversion()
    {
        if (!isGravityInvertedPressedPrev && isGravityInvertedPressed && isFootOnGround)
        {
            transform.Rotate(0, 0, 180f);
            isGravityInverted = !isGravityInverted;
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

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        baseGravity = (2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;

        currentGravity = baseGravity;
        Debug.Log("timeToApex = " + timeToApex);
        Debug.Log("gravity = " + baseGravity);
        Debug.Log("initialJumpVelocity = " + initialJumpVelocity);
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * moveSpeed;
        isMovementPressed = currentMovementInput.x != 0;
    }

    void onInvertGravity(InputAction.CallbackContext context)
    {
       isGravityInvertedPressed = context.ReadValueAsButton();
    }

    void setupIsGrounded()
    {
        float characterColliderOffset = 0.9f;
        float characterHeight = 1.9f;
        //float characterRadius = 0.3f;
        headPosition = Vector3.zero;
        headPosition.y += characterColliderOffset + (characterHeight/2) ;
        feetPosition = Vector3.zero;
        feetPosition.y += characterColliderOffset - (characterHeight/2); 
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
