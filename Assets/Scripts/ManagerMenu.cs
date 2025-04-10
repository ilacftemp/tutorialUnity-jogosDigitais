using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        MusicManager.Instance?.FadeToVolume(0.3f, 2f);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToStartMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}