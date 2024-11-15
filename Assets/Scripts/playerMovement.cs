using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheckBot;
    public Transform groundCheckTop;
    public string groundLayer = "Ground"; // Change to your ground tag

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -9.81f, 0);
    }

    void Update()
    {
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
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        movement.Normalize();

        // Rotate character to face movement direction
        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Apply movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        // Check if the character is on the ground
        isGrounded = Physics.CheckSphere(groundCheckBot.position, 0.1f, LayerMask.GetMask(groundLayer)) ||
                      Physics.CheckSphere(groundCheckTop.position, 0.1f, LayerMask.GetMask(groundLayer));
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(-Physics.gravity /9.81f * jumpForce, ForceMode.Impulse);
           
        }
    }
    private void InvertGravity()
    {
        // Invert gravity
        Physics.gravity = -Physics.gravity;
    }
}
