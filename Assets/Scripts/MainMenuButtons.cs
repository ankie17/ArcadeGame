using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuButtons : MonoBehaviour
{
    [SerializeField]
    private GameObject aboutText;
    [SerializeField]
    private GameObject continueCampaighButton;
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
    public void LoadAILevel()
    {
        SceneManager.LoadScene("ChooseLevelScene");
    }
    public void DemoMode()
    {
        SceneManager.LoadScene("demon");
    }
}
