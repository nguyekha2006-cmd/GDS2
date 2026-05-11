using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            GameManager.Instance.SetCheckpoint(transform, this);

            ActivateVisual();
        }
    }

    public void ActivateVisual()
    {
        if (sr != null)
            sr.color = Color.green;
    }

    public void Deactivate()
    {
        activated = false;

        if (sr != null)
            sr.color = Color.white;
    }
}