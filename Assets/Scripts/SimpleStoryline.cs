using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleStoryline : MonoBehaviour
{
    [SerializeField]
    private int IntroOrOutro;
    [SerializeField]
    private GameObject[] slides;
    [SerializeField]
    private float showTime = 5.0f;
    private float currentTime = 0f;
    private int currentSlideId = 0;
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= showTime)
        {
            currentTime = 0;
            if (currentSlideId == 0)
            {
                slides[currentSlideId].SetActive(false);
                currentSlideId = 1;
            }
            else if (currentSlideId == 1)
            {
                slides[currentSlideId].SetActive(false);
                if (IntroOrOutro == 1)
                {
                    currentSlideId = 2;
                }
                else
                {
                    PlayerPrefs.DeleteKey("CurrentLevelId");
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("MainMenu");
                }
            }
            else if (currentSlideId == 2)
            {
                SceneManager.LoadScene("level1");
            }
        }
    }
}
