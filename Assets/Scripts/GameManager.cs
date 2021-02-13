using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int PlayerMaxLives;
    private int playerCurrentLives;
    private Vector3 spawnPosition;
    public int StarsQuantity;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPosition = player.transform.position;

        var stars = GameObject.FindGameObjectsWithTag("Star");
        StarsQuantity = stars.Length;

        playerCurrentLives = PlayerMaxLives;
    }

    public void StarPickup()
    {
        StarsQuantity--;
        if (StarsQuantity == 0)
        {
            //complete level
            Debug.Log("Level complete");
        }
    }
    public void PlayerHurt()
    {
        playerCurrentLives--;
        if (playerCurrentLives > 0)
        {
            player.transform.position = spawnPosition;
            player.GetComponent<PlayerController>().Respawn();
        }
        else
        {
            //player dead
            Object.Destroy(player);
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
    }
}
