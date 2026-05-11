using UnityEngine;

public class PlayerCollisionPush : MonoBehaviour
{
    public float pushMultiplier = 1.5f;
    public float minSwingSpeed = 2f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb == null) return;

        if (rb.linearVelocity.magnitude < otherRb.linearVelocity.magnitude)
            return;

        Vector2 myVel = rb.linearVelocity;

        float speed = myVel.magnitude;

        if (speed < minSwingSpeed) return;

        Vector2 pushDir = myVel.normalized;

        otherRb.linearVelocity += pushDir * speed * pushMultiplier;
    }
}