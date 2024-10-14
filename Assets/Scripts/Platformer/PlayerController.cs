using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Rigidbody2D))]  // Ensures that Rigidbody2D is always attached
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float jumpForce = 10f;
        
        private Rigidbody2D rb;
        private bool isGrounded;

        [Header("Ground Check Settings")]
        public Transform groundCheck;
        public float groundCheckRadius = 0.2f;
        public LayerMask groundLayer;

        // Start is called before the first frame update
        void Start()
        {
            // Get the Rigidbody2D component
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
            HandleJump();
        }

        void HandleMovement()
        {
            // Get horizontal input
            float moveInput = InputSystem.HorizontalRaw();
            
            // Move the player horizontally
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        void HandleJump()
        {
            // Check if the player is on the ground
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // Handle jumping
            if (isGrounded && InputSystem.Jump())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        // This is to visualize the ground check in the scene view (optional)
        void OnDrawGizmos()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
}
