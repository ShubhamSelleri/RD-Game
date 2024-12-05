using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;




public class characterScript : MonoBehaviour
{

    public float rotationFactorPerFrame = 15.0f;
    public float moveSpeed = 10f;
    public float gravity = 9.81f;
    public float maxVerticalSpeed = 10f;

    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    private int RunHash;
    private int JumpHash;
    private int FallingHash;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;

    private bool isMovementPressed;
    private bool isGravityInverted;

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
    }

    void onJump(InputAction.CallbackContext context)
    {
        bool isJumpPressed = context.ReadValueAsButton();
        if (isJumpPressed && characterController.isGrounded)
        {

        }
    }


    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        isMovementPressed = currentMovementInput.x != 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        handleGravity();
        characterController.Move(currentMovement * moveSpeed * Time.deltaTime);
        handleAnimation();
        handleRotation();

    }

    void handleAnimation()
    {
        bool isRunning = animator.GetBool(RunHash);
        bool isJumping = animator.GetBool(JumpHash);
        bool isFalling = animator.GetBool(FallingHash);

        if (isMovementPressed && !isRunning)
        {
            animator.SetBool(RunHash, true);
        }
        else if (!isMovementPressed && isRunning)
        {
            animator.SetBool(RunHash, false);
        }
    }

    void handleGravity()
    {
        if(characterController.isGrounded)
        {
            float groundedGravity = -.05f;
            currentMovement.y = groundedGravity;
        }
        else
        {
            //change sign of gravity if needed

            bool isMaxVerticalSpeedExceeded = isGravityInverted ? (currentMovement.y - gravity * Time.deltaTime) > maxVerticalSpeed :
                                                                  (currentMovement.y + gravity * Time.deltaTime) < maxVerticalSpeed ;

            if (isGravityInverted)
            {
                if (isMaxVerticalSpeedExceeded)
                {
                    currentMovement.y = maxVerticalSpeed;
                }
                else
                {
                    currentMovement.y += gravity * Time.deltaTime;
                }
            }
            else
            {
                if (isMaxVerticalSpeedExceeded)
                {
                    currentMovement.y = -maxVerticalSpeed;
                }
                else
                {
                    currentMovement.y -= gravity * Time.deltaTime;
                }
            }
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
