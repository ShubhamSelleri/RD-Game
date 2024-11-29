using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheckBot;
    public Transform groundCheckTop;
    public string groundLayer = "Ground"; 

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 velocity;
    private CharacterController characterController;

    private float jumpStartTime;
    private float delayJump=0.5f;

    private int velovityMultiplier=1;

    public Animator animator;
    public float gravity=9.81f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -gravity, 0);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheckBot.position, 0.1f, LayerMask.GetMask(groundLayer));
        
        if (isGrounded) {
            animator.SetBool("Falling", false);
        }
        else {
            animator.SetBool("Falling", true);
        }

        // If animator jump is true set to false
        if (animator.GetBool("Jump")) {
            animator.SetBool("Jump", false);
        }

        Move();
        Jump();
        if (Input.GetKeyDown(KeyCode.S))
        {
            InvertGravity();
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
    private void InvertGravity()
    {
        // Invert gravity
        velovityMultiplier = -velovityMultiplier;
        jumpStartTime=Time.time;
        transform.Rotate(0,0,180f);
    }
}
