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

    public bool canSwitchGravityMidAir = false;

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
    private bool isFalling;
    private bool isFootOnGround;
    private bool isHeadTouching = false;

    private Vector3 headPosition;
    private Vector3 feetPosition;
    private float characterRadius;
    private string groundLayer = "Ground";

    private Vector3 groundObjectLastPostion;
    private Vector3 groundObjectCurrentPosition;

    private Quaternion groundObjectLastRotation;
    private Quaternion groundObjectCurrentRotation;
    

    private GameObject groundObject;
    private GameObject groundObjectPrev;

    private GameObject headObject;


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
        handleMovingPlatform();
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

        // movement animation
        if (isMovementPressed && !isAnimatorRunning && !isJumping)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if (!isMovementPressed && isAnimatorRunning)
        {
            animator.SetBool(isRunningHash, false);
        }

        // falling animation
        if (isFalling && !isFallingAnimating)
        {
            animator.SetBool(isFallingHash, true);
            isFallingAnimating = true;
        }
        else if (!isFalling && isFallingAnimating)
        {
            animator.SetBool(isFallingHash, false);
            isFallingAnimating = false;

            // vibrate controller on landing
            StartCoroutine(vibrateController(0.2f, 0.15f, 0.7f));
        }

        // jumping animation
        if (isFootOnGround && !isJumpPressed)
        {
            animator.SetBool(isJumpingHash, false);
            isJumpAnimating = false;
        }
        else if(isFootOnGround && isJumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;

            // vibrate controller on jump start

            //disable running animation
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

    void handleMovingPlatform()
    {
        if (groundObject != null && groundObject == groundObjectPrev)
        {
            groundObjectCurrentPosition = groundObject.transform.position;

            if((groundObjectLastPostion != groundObjectCurrentPosition)
                && isFootOnGround
                && (groundObjectLastPostion != Vector3.zero)) // fake null value that should never happen in the actual game
            {
                Vector3 platformMovement = groundObjectCurrentPosition - groundObjectLastPostion;

                
                
                characterController.Move(platformMovement);
            }

            // changes player position if object it's standing on rotates
            if (groundObjectLastRotation != groundObjectCurrentRotation
                    && isFootOnGround
                    && (groundObjectLastRotation != Quaternion.Euler(9f, 6f, 3f))) //fake null value that should never happen in the game
            {
                groundObjectCurrentRotation = groundObject.transform.rotation;
                Quaternion platFormRotation = Quaternion.Inverse(groundObjectCurrentRotation)*groundObjectLastRotation; //subtracts new rotation from old

                Vector3 relativePosition = groundObject.transform.position -(transform.position + feetPosition);

                characterController.Move(platFormRotation * relativePosition);
            }
            
        }
        else
        {
            groundObjectCurrentPosition = Vector3.zero;
            groundObjectCurrentRotation = Quaternion.Euler(9f, 6f, 3f);
        }
        groundObjectPrev = groundObject;
        groundObjectLastRotation = groundObjectCurrentRotation;
        groundObjectLastPostion = groundObjectCurrentPosition;
    }

    void handleGravity()
    {
        float groundedGravity = 0.05f;

        if (isFootOnGround)
        {
            currentMovement.y = isGravityInverted ? groundedGravity : -groundedGravity;
        }

        //checks Y velocity depending on gravity and if jump is released
        isFalling = (isGravityInverted ? currentMovement.y > groundedGravity: currentMovement.y < -groundedGravity) || (!isJumpPressed && !isFootOnGround);

        if (canSwitchGravityMidAir)
        {
            currentMovement.y = calculateGravityFloatingSwitch(isFalling, currentMovement.y);
        }
        else
        {
            currentMovement.y =  calculateGravityGroundedSwitch(isFalling, currentMovement.y);
        }

        
    }

    float calculateGravityGroundedSwitch(bool isFalling, float currentVerticalVelocity)
    {
        float previousYVelocity = currentVerticalVelocity;
        float newYVelocity;
        float nextYVelocity;
        if (isFalling)
        {
            // do not change this, doing it this way makes the jump frame rate independand
            newYVelocity = currentMovement.y;
            newYVelocity += isGravityInverted ? currentGravity * fallMultiplier * Time.deltaTime :
                                                -currentGravity * fallMultiplier * Time.deltaTime;
            nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;

        }
        //else the character is still jumping "up"
        else
        {
            newYVelocity = currentMovement.y;
            newYVelocity += isGravityInverted ? currentGravity * Time.deltaTime :
                                                -currentGravity * Time.deltaTime;
            nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
        }

        return nextYVelocity;
    }

    float calculateGravityFloatingSwitch(bool isFalling, float currentVerticalVelocity)
    {
        float previousYVelocity = currentVerticalVelocity;
        float newYVelocity;
        float nextYVelocity;

        if (isFalling)
        {
            // do not change this, doing it this way makes the jump frame rate independand
            newYVelocity = currentMovement.y;
            newYVelocity += isGravityInverted ? currentGravity * fallMultiplier * gravityFloatingMultiplier *  Time.deltaTime :
                                                -currentGravity * fallMultiplier * gravityFloatingMultiplier * Time.deltaTime;
            nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;

        }
        //else the character is still jumping "up"
        else
        {
            newYVelocity = currentMovement.y;
            newYVelocity += isGravityInverted ? currentGravity * Time.deltaTime :
                                                -currentGravity * Time.deltaTime;
            nextYVelocity = (currentMovement.y + newYVelocity) * 0.5f;
        }
        return nextYVelocity;
    }

    void handleIsGrounded()
    {
        Vector3 headDetectionCenter = transform.position + headPosition;
        headDetectionCenter.y +=  -characterRadius/2 + 0.05f;

        Vector3 feetDetectionCenter = transform.position + feetPosition;
        feetDetectionCenter.y +=  characterRadius/2 - 0.05f;

        int groundLayerMask = LayerMask.GetMask(groundLayer);

        Collider[] feetColliders = Physics.OverlapSphere(feetDetectionCenter, characterRadius / 2, groundLayerMask);
        Collider[] headColliders = Physics.OverlapSphere(headDetectionCenter, characterRadius / 2, groundLayerMask);

        // Determine if foot is on the ground and store the object
        isFootOnGround = feetColliders.Length > 0;
        groundObject = isFootOnGround ? feetColliders[0].gameObject : null;

        // Determine if head is touching and store the object
        isHeadTouching = headColliders.Length > 0;
        headObject = isHeadTouching ? headColliders[0].gameObject : null;
    }

    void handleGravityInversion()
    {
        Debug.Log(transform.position);
        if (!isGravityInvertedPressedPrev && isGravityInvertedPressed && (canSwitchGravityMidAir || isFootOnGround))
        {
            transform.position += Vector3.up * 0.9f; //character offset
            transform.Rotate(0, 0, 180f);
            isGravityInverted = !isGravityInverted;
            if (isFalling)
            {
                gravityFloatingMultiplier *= 1.1f;
            }

            // reset gravityFloatingMultiplier when landing
            if (isFootOnGround)
            {
                gravityFloatingMultiplier = 1;
            }
        }

        isGravityInvertedPressedPrev = isGravityInvertedPressed;
        

    }

    void handleRotation()
    {
        if (isMovementPressed)
        {
            if (currentMovementInput.x > 0.2f)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 90, transform.eulerAngles.z);
            }
            else if (currentMovementInput.x < -0.2f)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, -90, transform.eulerAngles.z);
            }
        }
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    IEnumerator vibrateController(float duration, float freq1, float freq2)
    {
        float startTime = Time.time;
        InputSystem.ResumeHaptics();
        while (Time.time < startTime + duration)
        {
            Gamepad.current.SetMotorSpeeds(freq1, freq2);
            yield return null;
        }
        InputSystem.PauseHaptics();
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
        characterRadius = 0.3f;

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
