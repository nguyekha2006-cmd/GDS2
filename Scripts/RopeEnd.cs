using UnityEngine;
using UnityEngine.InputSystem;

public class RopeEnd : MonoBehaviour
{
    public bool isPlayerInside = false;
    public bool isAttached = false;

    public Transform currentPlayer;
    public Transform anchor;
    public float ropeLength = 4f;

    public RopeEnd otherEnd;

    Rigidbody2D playerRb;

    float attachCooldown = 0f;

    [Header("Pulley Settings")]
    public float totalLength = 8f;
    public float pulleyForce = 6f;

    [Header("Tuning")]
    public float damping = 0.995f;
    public float climbForce = 5f;
    public float swingForce = 70f;

    public LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            currentPlayer = other.transform;
            playerRb = currentPlayer.GetComponent<Rigidbody2D>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == currentPlayer)
        {
            if (isAttached) return;

            isPlayerInside = false;
            currentPlayer = null;
            playerRb = null;
        }
    }

    void Update()
    {
        HandleAttachInput();

        if (isAttached && currentPlayer != null && anchor != null)
        {
            ApplyConstraint();
            HandleSwing();
        }

        attachCooldown -= Time.deltaTime;

        UpdateRopeVisual();
    }

    void HandleAttachInput()
    {
        if (!isPlayerInside || currentPlayer == null || attachCooldown > 0f)
            return;

        var m = currentPlayer.GetComponent<MisakiMove>();
        var f = currentPlayer.GetComponent<FuyukiMove>();

        bool interact = false;

        if (m != null) interact = m.interactPressed;
        if (f != null) interact = f.interactPressed;

        if (interact)
        {
            if (!isAttached)
                Attach();
            else
                Detach();

            attachCooldown = 0.2f;
        }
    }

    void HandleSwing()
    {
        Rigidbody2D rb = playerRb;
        if (rb == null) return;

        Vector2 dir = (currentPlayer.position - anchor.position).normalized;
        Vector2 tangent = new Vector2(-dir.y, dir.x);

        float input = 0;

        var m = currentPlayer.GetComponent<MisakiMove>();
        var f = currentPlayer.GetComponent<FuyukiMove>();

        if (m != null) input = m.horizontalInput;
        if (f != null) input = f.horizontalInput;

        float swingFactor = Mathf.Clamp(Mathf.Abs(dir.x), 0.3f, 1f);

        if (Mathf.Abs(input) > 0)
        {
            rb.AddForce(tangent * input * swingForce * swingFactor);
        }
    }

    void ApplyConstraint()
    {
        if (!isAttached || otherEnd == null || !otherEnd.isAttached || otherEnd.currentPlayer == null)
            return;

        Rigidbody2D rbA = playerRb;
        Rigidbody2D rbB = otherEnd.playerRb;

        if (rbA == null || rbB == null) return;

        Vector2 dirA = currentPlayer.position - anchor.position;
        Vector2 dirB = otherEnd.currentPlayer.position - anchor.position;

        float distA = dirA.magnitude;
        float distB = dirB.magnitude;

        float total = distA + distB;

        Vector2 ropeDirA = dirA.normalized;
        Vector2 ropeDirB = dirB.normalized;

        float downA = Vector2.Dot(rbA.linearVelocity, ropeDirA);
        float downB = Vector2.Dot(rbB.linearVelocity, ropeDirB);

        float excess = 0f;

        // ===== PULLEY =====
        if (total > totalLength)
        {
            excess = total - totalLength;

            float force = excess * pulleyForce;

            if (downA > downB)
            {
                rbB.AddForce(Vector2.up * force, ForceMode2D.Force);
            }
            else if (downB > downA)
            {
                rbA.AddForce(Vector2.up * force, ForceMode2D.Force);
            }
        }

        // ===== CLIMB =====
        float inputA = 0;
        float inputB = 0;

        var mA = currentPlayer.GetComponent<MisakiMove>();
        var fA = currentPlayer.GetComponent<FuyukiMove>();

        var mB = otherEnd.currentPlayer.GetComponent<MisakiMove>();
        var fB = otherEnd.currentPlayer.GetComponent<FuyukiMove>();

        if (mA != null) inputA = mA.verticalInput;
        if (fA != null) inputA = fA.verticalInput;

        if (mB != null) inputB = mB.verticalInput;
        if (fB != null) inputB = fB.verticalInput;

        rbA.AddForce(-ropeDirA * inputA * climbForce * rbA.mass);
        rbB.AddForce(-ropeDirB * inputB * climbForce * rbB.mass);

        // ===== RADIAL CONTROL (KHÔNG CHẶN 100%) =====
        float outA = Vector2.Dot(rbA.linearVelocity, ropeDirA);
        float outB = Vector2.Dot(rbB.linearVelocity, ropeDirB);

        if (outA > 0)
            rbA.linearVelocity -= ropeDirA * outA * 0.7f;

        if (outB > 0)
            rbB.linearVelocity -= ropeDirB * outB * 0.7f;

        // ===== DAMPING =====
        rbA.linearVelocity *= damping;
        rbB.linearVelocity *= damping;

        // ===== LIMIT SPEED =====
        float maxSpeed = 10f;

        rbA.linearVelocity = Vector2.ClampMagnitude(rbA.linearVelocity, maxSpeed);
        rbB.linearVelocity = Vector2.ClampMagnitude(rbB.linearVelocity, maxSpeed);
    }

    void Attach()
    {
        isAttached = true;

        SetPlayerAttachState(true);

        Vector2 dir = (currentPlayer.position - anchor.position).normalized;

        if (dir.magnitude < 0.01f)
            dir = Vector2.right;

        currentPlayer.position = anchor.position + (Vector3)(dir * ropeLength);

        if (playerRb != null)
            playerRb.linearVelocity = Vector2.zero;
    }

    void Detach()
    {
        isAttached = false;
        SetPlayerAttachState(false);
    }

    void SetPlayerAttachState(bool state)
    {
        if (currentPlayer == null) return;

        var m = currentPlayer.GetComponent<MisakiMove>();
        if (m != null) m.isAttachedToRope = state;

        var f = currentPlayer.GetComponent<FuyukiMove>();
        if (f != null) f.isAttachedToRope = state;

        var rb = currentPlayer.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (state)
                rb.sharedMaterial = null; 
        }
    }

    public bool BothPlayersAttached()
    {
        return isAttached && otherEnd != null && otherEnd.isAttached;
    }

    void OnDrawGizmos()
    {
        if (anchor != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(anchor.position, ropeLength);
        }
    }

    void UpdateRopeVisual()
    {
        if (line == null) return;

        if (isAttached && currentPlayer != null)
        {
            line.enabled = true;

            int segments = 12;

            line.positionCount = segments;

            Vector3 start = anchor.position;
            Vector3 end = currentPlayer.position;

            for (int i = 0; i < segments; i++)
            {
                float t = i / (float)(segments - 1);

                Vector3 point = Vector3.Lerp(start, end, t);

                float tension = Mathf.Clamp01(
                    Vector2.Distance(start, end) / ropeLength
                );

                float sagAmount = Mathf.Lerp(0.7f, 0.1f, tension);

                float sag = Mathf.Sin(t * Mathf.PI) * sagAmount;

                point.y -= sag;

                line.SetPosition(i, point);
            }
        }
        else
        {
            line.enabled = false;
        }
    }
}