using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;

public class AIManager : MonoBehaviour
{
    private EnemyAIController [] enemies = new EnemyAIController[3];
    [SerializeField]
    private GameObject aiEnemyPrefab;
    private string serverResponse = "";
    private bool requestStatus = false;
    HttpWebRequest webRequest;
    public bool Paused = false;
    void StartWebRequest()
    {
        var url = "http://pacman-npc-orchestrator.herokuapp.com/get_strategy";

        webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.Method = "POST";

        webRequest.ContentType = "application/json";

        var data = FindObjectOfType<LevelBuilder>().GetLevelMatrix().GetJsonString();
        using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
        {
            streamWriter.Write(data);
        }

        webRequest.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);
    }
    void FinishWebRequest(IAsyncResult result)
    {
        using (var streamReader = new StreamReader(webRequest.EndGetResponse(result).GetResponseStream()))
        {
            serverResponse =  streamReader.ReadToEnd();
        }
    }
    public void SpawnEnemies()
    {
        var coins = GameObject.FindGameObjectsWithTag("Star");
        int counter = 0;
        foreach(var c in coins)
        {
            var enemy = Instantiate(aiEnemyPrefab, c.transform.position, Quaternion.identity).GetComponent<EnemyAIController>();
            enemy.Id = counter + 3;
            enemy.moveSpeed = enemy.Id;
            enemies[counter] = enemy;
            counter++;
        }
    }
    private void FixedUpdate()
    {
        if (enemies != null)
        {
            if (!requestStatus)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    requestStatus = true;
                    StartWebRequest();
                }
            }
            else
            {
                if (serverResponse != "")
                {
                    foreach (var c in enemies)
                    {
                        c.ValueMatrix = ParseValueMatrix(c.Id);
                        if (!Paused)
                            c.EnemyUnpause();
                    }
                    serverResponse = "";
                    requestStatus = false;
                }
            }
        }
    }
    private double[,] ParseValueMatrix(int enemyId)
    {
        JObject obj = JObject.Parse(serverResponse);
        double[,] vm = new double[20, 20];
        int counter = 0;
        foreach (var p in obj.Property($"{enemyId}"))
        {
            foreach (var s in p)
            {
                foreach (var l in s)
                {
                    vm[counter / 20, counter % 20] = (double)l;
                    counter++;
                }
            }
        }
        return vm;
    }
}
