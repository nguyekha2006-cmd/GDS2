using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform fuyuki;
    public Transform misaki;
    public Transform respawnPoint;
    public CameraFollow2Players cam;

    void Awake()
    {
        Instance = this;
    }

    public void RespawnBothPlayers()
    {
        fuyuki.position = respawnPoint.position + Vector3.left * 1f;
        misaki.position = respawnPoint.position + Vector3.right * 1f;

        ResetVelocity(fuyuki);
        ResetVelocity(misaki);

        cam.SnapToTarget();
    }

    void ResetVelocity(Transform player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}