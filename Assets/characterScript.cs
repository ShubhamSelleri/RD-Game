using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;




public class characterScript : MonoBehaviour
{

    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;


    private Vector2 currentMovementInput;
    private Vector3 currentMovement;

    private bool isMovementPressed;

    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();


        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
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
        characterController.Move(currentMovement * Time.deltaTime);
        handleAnimation();

    }

    void handleAnimation()
    {
        bool isRunning = animator.GetBool("Run");
        bool isJumping = animator.GetBool("Jump");
        bool isFalling = animator.GetBool("Falling");

        if (isMovementPressed && !isRunning)
        {
            animator.SetBool("Run", true);
        }
        else if ( !isMovementPressed && isRunning)
        {
            animator.SetBool("Run", false);
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
