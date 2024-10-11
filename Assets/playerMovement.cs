using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the player
    public float jumpForce = 10f;  // Force applied when jumping
    private Rigidbody rb;
    private bool isGrounded;
    private ResetPositions ResetInstance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetInstance = GameObject.Find("gameManager").GetComponent<ResetPositions>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Get horizontal input
        float moveVertical = Input.GetAxis("Vertical"); // Get vertical input

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical); // Create movement vector
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime); // Move player
    }

    private void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) // Check if grounded and space is pressed
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse); // Apply jump force
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Check if player is touching the ground
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Check if player is leaving the ground
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Red")){
            ResetInstance.ResetPositionsToInitial();
        }
    }
}
