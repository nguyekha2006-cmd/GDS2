using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform fuyuki;
    public Transform misaki;

    public Transform currentCheckpoint;
    public Checkpoint currentCheckpointScript;

    public CameraFollow2Players cam;

    void Awake()
    {
        Instance = this;
    }

    public void RespawnBothPlayers()
    {
        if (currentCheckpoint == null)
        {
            Debug.LogError("No checkpoint set!");
            return;
        }

        Vector3 spawnPos = currentCheckpoint.position;

        fuyuki.position = spawnPos + Vector3.left * 1f;
        misaki.position = spawnPos + Vector3.right * 1f;

        ResetVelocity(fuyuki);
        ResetVelocity(misaki);

        if (cam != null)
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

    public void SetCheckpoint(Transform checkpoint, Checkpoint checkpointScript)
    {
        if (currentCheckpointScript != null)
        {
            currentCheckpointScript.Deactivate();
        }

        currentCheckpoint = checkpoint;
        currentCheckpointScript = checkpointScript;
    }
}