using UnityEngine;
using UnityEngine.InputSystem;

public class FuyukiMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isGrounded;

    public bool isAttachedToRope = false;

    public bool isClimbing = false;
    float originalGravity;

    public bool interactPressed;
    public float horizontalInput;

    public float verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
    }

    void Update()
    {
        float moveX = 0;

        if (Keyboard.current.aKey.isPressed) moveX = -1;
        if (Keyboard.current.dKey.isPressed) moveX = 1;

        horizontalInput = moveX;
        interactPressed = Keyboard.current.eKey.wasPressedThisFrame;

        float vertical = 0;

        if (Keyboard.current.wKey.isPressed) vertical = 1;
        if (Keyboard.current.sKey.isPressed) vertical = -1;

        verticalInput = vertical;

        if (isAttachedToRope && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.98f, rb.linearVelocity.y);
            return;
        }

        if (isClimbing)
        {
            rb.linearVelocity = new Vector2(moveX * moveSpeed, vertical * moveSpeed);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        }

        if (Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = originalGravity;
        }
    }
}