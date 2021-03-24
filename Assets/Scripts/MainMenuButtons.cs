using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuButtons : MonoBehaviour
{
    public GameObject aboutText;
    public void LoadHighScoreScene()
    {
        SceneManager.LoadScene("HighScoreScene");
    }
    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("level1");
    }
    public void AboutText()
    {
        aboutText.SetActive(!aboutText.active);
    }
}
