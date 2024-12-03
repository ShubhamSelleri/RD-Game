using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;





public class characterScript : MonoBehaviour
{
    public Animator animator;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;
    public float maxVerticalSpeed = 10f;

    private CharacterController charControl;

    private Vector3 totalMovement;

    private Vector3 externalMovement;
    private Vector3 controllerMovement;

    private Vector3 gravityMovement;
    private Vector3 gravityAcceleration;

    private bool isGrounded;
    private bool isGravityInverted;

    // Start is called before the first frame update
    void Start()
    {
        //initialize all movement as zero
        totalMovement       = Vector3.zero;
        externalMovement    = Vector3.zero;
        controllerMovement  = Vector3.zero;
        gravityMovement     = Vector3.zero;
        gravityAcceleration = Vector3.up * gravity;

        //initialize others
        isGrounded = false;
        isGravityInverted = false;
    }

    // Update is called once per frame
    void Update()
    {
        updatesIsGrounded();
        Move();
    }

    private void Move()
    {
        updateGravityMovement();
        controllerMovement =  getControllerMovement();

        setCharacterDirection(controllerMovement);

        //adds all movemnt
        totalMovement = controllerMovement * moveSpeed + externalMovement + gravityMovement;

        //executes movement
        charControl.Move(controllerMovement * moveSpeed* Time.deltaTime);

        //reset movement
        controllerMovement = Vector3.zero;
        externalMovement = Vector3.zero;
    }

    private void setCharacterDirection(Vector3 controllerInput)
    {
        // Rotate character to face movement direction
        if (controllerMovement.magnitude > 0.1f)
        {
            if (controllerMovement.x > 0)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 90, transform.eulerAngles.z);
            }
            else if (controllerMovement.x < 0)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, -90, transform.eulerAngles.z);
            }
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    public void addCharacterSpeed(Vector3 speedToAdd)
    {
        externalMovement += speedToAdd;
    }

    private Vector3 getControllerMovement()
    {
        return controllerMovement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
    }

    private void updateGravityMovement()
    {
        if (isGrounded)
        {
            gravityMovement = Vector3.zero;
        }
        else
        {
            if(isGravityInverted)
            {
                gravityMovement += Vector3.up * gravity;
            }
            else
            {
                gravityMovement += Vector3.down * gravity;
            }
        }
    }
    private void updatesIsGrounded()
    {
        if(1 == 1)
        {
            isGrounded = true;
        }
    }
}
