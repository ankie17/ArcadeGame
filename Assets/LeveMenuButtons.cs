using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeveMenuButtons : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Pause()
    {
        GameManager.Instance.PauseGame();
    }
    public void Resume()
    {
        GameManager.Instance.ResumeGame();
    }
    public void Reload()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
