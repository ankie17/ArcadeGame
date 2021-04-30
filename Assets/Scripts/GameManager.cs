using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        Normal,
        AI
    }

    public GameMode gameMode;
    public int levelId;
    private static GameManager instance;
    public int PlayerMaxLives;
    private int playerCurrentLives;
    public int StarsQuantity;
    private GameObject player;
    private GameObject heart;
    private LevelTimer timer;
    private FXManager fXManager;
    private HighScoreTableWriter tableWriter;
    public GameObject respawnMenu;
    public static GameManager Instance { get { return instance; } }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        FindAndAssignKeyGameObjects();
    }
    private void FindAndAssignKeyGameObjects()
    {
        fXManager = FindObjectOfType<FXManager>();
        player = GameObject.FindGameObjectWithTag("Player");

        heart = GameObject.FindGameObjectWithTag("Heart");
        heart.SetActive(false);
        var stars = GameObject.FindGameObjectsWithTag("Star");
        StarsQuantity = stars.Length;

        timer = GetComponent<LevelTimer>();

        tableWriter = GetComponent<HighScoreTableWriter>();

        playerCurrentLives = PlayerMaxLives;

        //write progress
        PlayerPrefs.SetInt("CurrentLevelId", levelId);
        PlayerPrefs.Save();
    }
    public void StarPickup()
    {
        StarsQuantity--;

        if (StarsQuantity >= 1)
        {
            fXManager.PlayOneShot(0);
        }
        if (StarsQuantity == 0)
        {
            //complete level
            fXManager.PlayOneShot(3);
            Debug.Log("Level complete");
            
            GameEnd(true);
        }
        if (StarsQuantity == 1)
        {
            //activate heart
            ActivateHeart();
        }
    }
    public void PlayerHurt()
    {
        playerCurrentLives--;
        if (playerCurrentLives > 0)
        {
            player.GetComponent<PlayerController>().PlayDead(true);
            Debug.Log("respawn menu");
            respawnMenu.SetActive(true);
        }
        else
        {
            //player dead
            //loose sound
            fXManager.PlayOneShot(2);
            player.GetComponent<PlayerController>().PlayDead(false);
            GameEnd(false);
        }
    }

    public void PauseGame()
    {
        if (player != null)
        {
            player.GetComponent<PlayerController>().PausePlayer();
        }

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (gameMode == GameMode.Normal)
            {
                enemy.GetComponent<EnemyController>().EnemyPause();
            }
            else
            {
                enemy.GetComponent<EnemyAIController>().EnemyPause();
            }
        }

        if (gameMode == GameMode.AI) 
        {
            var aiManager = FindObjectOfType<AIManager>();
            aiManager.Paused = true;
        } 

        timer.switchStates();
    }
    public void ResumeGame()
    {
        if (player != null)
        {
            player.GetComponent<PlayerController>().UnpausePlayer();
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (gameMode == GameMode.Normal)
            {
                enemy.GetComponent<EnemyController>().EnemyUnpause();
            }
            else
            {
                enemy.GetComponent<EnemyAIController>().EnemyUnpause();
            }
        }

        if (gameMode == GameMode.AI)
        {
            var aiManager = FindObjectOfType<AIManager>();
            aiManager.Paused = false;
        }

        timer.switchStates();
    }

    public void GameEnd(bool result)
    {
        if (result)
        {
            PauseGame();
            player.SetActive(false);
            //запустить окно ввода никнейма
            if (levelId == 10)
            {
                var menu = FindObjectOfType<NicknameMenu>();
                menu.nickNameMenu.SetActive(true);
            }
            else
            {
                //level transition
                StartCoroutine(LevelTransition());
            }
        }
        else
        {
            StartCoroutine(LevelReload());
        }
    }
    private IEnumerator LevelTransition()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene($"level{levelId + 1}");
    }
    private IEnumerator LevelReload()
    {
        yield return new WaitForSeconds(2.0f);
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void LogPlayerStats(string name)
    {
        tableWriter.WritePlayerStats(name, (int)timer.levelTime);

        SceneManager.LoadScene("Outro");
    }
    public void HeartPickup()
    {
        fXManager.PlayOneShot(0);
        playerCurrentLives++;
    }
    public void ActivateHeart()
    {
        heart.SetActive(true);
    }
}
