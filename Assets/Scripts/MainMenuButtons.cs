using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuButtons : MonoBehaviour
{
    public GameObject aboutText;
    public GameObject continueCampaighButton;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("CurrentLevelId"))
        {
            continueCampaighButton.SetActive(false);
        }
        else
        {
            continueCampaighButton.SetActive(true);
        }
    }
    public void LoadHighScoreScene()
    {
        SceneManager.LoadScene("HighScoreScene");
    }
    public void StartNewGame()
    {
        PlayerPrefs.SetInt("CurrentLevelId", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Intro");
    }
    public void ContinueCampaign()
    {
        int currentLevelId = PlayerPrefs.GetInt("CurrentLevelId");
        SceneManager.LoadScene($"level{currentLevelId}");
    }
    public void AboutText()
    {
        aboutText.SetActive(!aboutText.activeInHierarchy);
    }
}
