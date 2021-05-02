using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorylineManager : MonoBehaviour
{
    [SerializeField]
    private GameObject firstSlide;
    [SerializeField]
    private GameObject secondSlide;
    [SerializeField]
    private GameObject sidePanel;
    [SerializeField]
    private float showTime = 5.0f;
    private float currentTime = 0f;
    private int currentSlideId = 0;

    private void Start()
    {
        GameManager.Instance.PauseGame();
        int levelId = GameManager.Instance.levelId;
        LoadSlidesFromResources(levelId);
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= showTime)
        {
            currentTime = 0;
            if (currentSlideId == 0)
            {
                firstSlide.SetActive(false);
                currentSlideId = 1;
            }
            else if (currentSlideId == 1)
            {
                secondSlide.SetActive(false);
                sidePanel.SetActive(false);
                GameManager.Instance.ResumeGame();
                Destroy(gameObject);
            }
        }
    }
    private void LoadSlidesFromResources(int levelId)
    {
        var firstImage = Resources.Load<Sprite>($"pre_{levelId}_1");
        var secondImage = Resources.Load<Sprite>($"pre_{levelId}_2");
        firstSlide.GetComponent<Image>().sprite = firstImage;
        secondSlide.GetComponent<Image>().sprite = secondImage;
    }
}
