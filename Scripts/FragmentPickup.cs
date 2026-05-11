using UnityEngine;

public class FragmentPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerIdentity player = other.GetComponent<PlayerIdentity>();

            if (player != null && player.isFuyuki)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        gameObject.SetActive(false);
    }
}