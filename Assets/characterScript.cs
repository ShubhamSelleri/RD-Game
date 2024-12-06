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

    private int RunningHash;
    private int JumpingHash;
    private int FallingHash;

    private bool isJumpAnimating;
    private bool isFallingAnimating;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;

    private bool isMovementPressed;
    private bool isGravityInverted = false;
    private bool isJumpPressed = false;

    private SphereCollider headCollider;


    private void Awake()
    {
        // get components
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // convert animator parameter strings to hash for performance
        RunningHash = Animator.StringToHash("Running");
        JumpingHash = Animator.StringToHash("Jumping");
        FallingHash = Animator.StringToHash("Falling");

        // listen to inputs
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;

        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;


        setupJumpVariables();
        // set maxVertical speed to the highest minimum value needed to make jump work
        maxVerticalSpeed = Mathf.Max(initialJumpVelocity,maxVerticalSpeed);

        setUpHeadCollider();
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

        handleGravity();
        //handleMaxVerticalSpeed();
        //handleJump();

    }

    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            animator.SetBool(JumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity * 0.5f; //asumes initial y velocity is 0;
        }
        else if(!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void handleAnimation()
    {
        bool isAnimatorRunning = animator.GetBool(RunningHash);
        bool isAnimatorJumping = animator.GetBool(JumpingHash);
        bool isAnimatorFalling = animator.GetBool(FallingHash);

        if (isMovementPressed && !isAnimatorRunning)
        {
            animator.SetBool(RunningHash, true);
        }
        else if (!isMovementPressed && isAnimatorRunning)
        {
            animator.SetBool(RunningHash, false);
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
        bool isFalling = (isGravityInverted ? currentMovement.y >= 0.0f : currentMovement.y < -0.05f);

        if(isFalling && !isFallingAnimating && !characterController.isGrounded)
        {
            animator.SetBool(FallingHash, true);
            isFallingAnimating = true;
        }
        else if(characterController.isGrounded && isFallingAnimating )
        {
            animator.SetBool(FallingHash, false);
            isFallingAnimating = false;
        }
        
        if(characterController.isGrounded)
        {
            float groundedGravity = -.05f;
            currentMovement.y = groundedGravity;
            animator.SetBool(JumpingHash, false);
            isJumpAnimating = false;

        }
        else if (isFalling)
        {
            animator.SetBool(FallingHash, true);
            animator.SetBool(JumpingHash, false);
            isJumpAnimating = false;

            float previousYVelocity = currentMovement.y;
            float newYVelocity;
            float nextYVelocity;
            //change sign of gravity if needed
            if (isGravityInverted)
            {
                newYVelocity = currentMovement.y + (gravity * fallMultiplier* Time.deltaTime);
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

    void handleRotation()
    {
        if (currentMovementInput.x > 0.1f)
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

    void handleGravityInversion()
    {

    }



    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
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

    void setUpHeadCollider()
    {
        Vector3 headPosition = characterController.center;
        
        headCollider = new SphereCollider();
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = headPosition;
        float scaleFactor = 0.3f;
        sphere.transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);

        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material.color = Color.red;
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
