using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScene : MonoBehaviour
{
    public void ResumeGame()
    {
        SceneManager.LoadScene(PauseData.previousScene);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}