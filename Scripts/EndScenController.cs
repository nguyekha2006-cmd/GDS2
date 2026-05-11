using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneController : MonoBehaviour
{
    public float waitTime = 5f;

    void Start()
    {
        Invoke("ReturnToMenu", waitTime);
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}