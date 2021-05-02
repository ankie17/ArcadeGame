using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private enum GameMode
    {
        Normal,
        AI
    }
    [SerializeField]
    private GameMode gameMode;
    public int levelId;
    private static GameManager instance;
    [SerializeField]
    private int PlayerMaxLives;
    private int playerCurrentLives;
    [SerializeField]
    private int StarsQuantity;
    private GameObject heart;
    public GameObject player;
    private LevelTimer timer;
    private FXManager fXManager;
    private HighScoreTableWriter tableWriter;
    [SerializeField]
    private GameObject respawnMenu;
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
        if (gameMode == GameMode.Normal)
            FindAndAssignKeyGameObjects();
        else
        {
            FindObjectOfType<LevelBuilder>().Prepare();
        }
    }
    public void FindAndAssignKeyGameObjects()
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
            fXManager.PlayOneShot(3);
            
            GameEnd(true);
        }
        if (StarsQuantity == 1)
        {
            ActivateHeart();
        }
    }
    public void PlayerHurt()
    {
        playerCurrentLives--;
        if (playerCurrentLives > 0)
        {
            player.GetComponent<PlayerController>().PlayDead(true);
            respawnMenu.SetActive(true);
        }
        else
        {
            fXManager.PlayOneShot(2);
            player.GetComponent<PlayerController>().PlayDead(false);
            GameEnd(false);
        }
    }
    public void PauseGame()
    {
        if (player.GetComponent<PlayerController>()!= null)
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

        timer.SwitchStates();
    }
    public void ResumeGame()
    {
        if (player.GetComponent<PlayerController>() != null)
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

        timer.SwitchStates();
    }
    public PlayerController FindPlayer()
    {
        return player.GetComponent<PlayerController>();
    }
    private void GameEnd(bool result)
    {
        if (result)
        {
            PauseGame();
            player.SetActive(false);

            if (levelId == 10||gameMode==GameMode.AI)
            {
                var menu = FindObjectOfType<NicknameMenu>();
                menu.NickNameMenu.SetActive(true);
            }
            else
            {
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

        if (gameMode == GameMode.Normal)
            SceneManager.LoadScene($"level{levelId + 1}");
        else
            SceneManager.LoadScene("MainMenu");
    }
    private IEnumerator LevelReload()
    {
        yield return new WaitForSeconds(2.0f);
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void LogPlayerStats(string name)
    {
        int time = (int)timer.levelTime;

        if (gameMode == GameMode.Normal)
        {
            tableWriter.WritePlayerStats(name, 0, time);
            SceneManager.LoadScene("Outro");
        }
        else
        {
            var difficulty = FindObjectOfType<LevelBuilder>().difficulty;

            if (difficulty == LevelDifficulty.easy)
            {
                tableWriter.WritePlayerStats(name, 1, time);
            }
            else if (difficulty == LevelDifficulty.medium)
            {
                tableWriter.WritePlayerStats(name, 2, time);
            }
            else
            {
                tableWriter.WritePlayerStats(name, 3, time);
            }

            StartCoroutine(LevelTransition());
        }
    }
    public void HeartPickup()
    {
        fXManager.PlayOneShot(0);
        playerCurrentLives++;
    }
    private void ActivateHeart()
    {
        heart.SetActive(true);
    }
}
