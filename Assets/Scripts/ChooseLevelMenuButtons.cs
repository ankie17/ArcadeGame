using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevelMenuButtons : MonoBehaviour
{
    public void LevelButton(string difficulty)
    {
        StartCoroutine(LoadSceneAsync("level" + difficulty));
    }
    IEnumerator LoadSceneAsync(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void BackToMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
