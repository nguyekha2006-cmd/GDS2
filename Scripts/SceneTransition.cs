using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int nextSceneIndex;
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            Invoke("LoadNextScene", 1f); 
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
}