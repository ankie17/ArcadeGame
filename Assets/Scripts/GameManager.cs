﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public int PlayerMaxLives;
    private int playerCurrentLives;
    public int StarsQuantity;
    private GameObject nicknameMenu;
    private GameObject player;
    private GameObject heart;
    private LevelTimer timer;
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
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nicknameMenu = FindObjectOfType<NicknameScipr>().gameObject;
        nicknameMenu.SetActive(false);
        heart = GameObject.FindGameObjectWithTag("Heart");
        heart.SetActive(false);
        var stars = GameObject.FindGameObjectsWithTag("Star");
        StarsQuantity = stars.Length;

        timer = GetComponent<LevelTimer>();

        tableWriter = GetComponent<HighScoreTableWriter>();

        playerCurrentLives = PlayerMaxLives;
    }
    public void StarPickup()
    {
        StarsQuantity--;
        if (StarsQuantity == 0)
        {
            //complete level
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
            player.SetActive(false);
            respawnMenu.SetActive(true);
        }
        else
        {
            //player dead
            Object.Destroy(player);
            GameEnd(false);
        }
    }

    public void PauseGame()
    {
        AudioSource audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();

        audioSource.Pause();

        player.GetComponent<PlayerController>().PausePlayer();

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<EnemyController>().EnemyPause();
        }

        timer.switchStates();
    }
    public void ResumeGame()
    {
        AudioSource audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();

        audioSource.Play();

        player.GetComponent<PlayerController>().UnpausePlayer();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<EnemyController>().EnemyUnpause();
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
            nicknameMenu.SetActive(true);
        }
    }
    public void LogPlayerStats(string name)
    {
        Debug.Log(name + " " + timer.levelTime);
        tableWriter.WritePlayerStats(name, timer.levelTime);
        SceneManager.LoadScene("HighScoreScene");
    }
    public void HeartPickup()
    {
        playerCurrentLives++;
    }
    public void ActivateHeart()
    {
        heart.SetActive(true);
    }
}
