using UnityEngine;

public class FakeWallDisappear : MonoBehaviour
{
    public GameObject hiddenArea;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hiddenArea.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}