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
    private EnemyAIController [] enemies;
    public string server = "";
    public string dataString;
    private bool requestStatus = false;
    HttpWebRequest webRequest;
    public string debugString;
    void StartWebRequest()
    {
        var url = "http://pacman-npc-orchestrator.herokuapp.com/get_strategy";

        webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.Method = "POST";

        webRequest.ContentType = "application/json";

        var data = FindObjectOfType<LevelBuilder>().GetLevelMatrix().GetJsonString();
        dataString = data;
        using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
        {
            streamWriter.Write(data);
        }

        webRequest.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);
    }

    void FinishWebRequest(IAsyncResult result)
    {
        //webRequest.EndGetResponse(result).GetResponseStream();
        using (var streamReader = new StreamReader(webRequest.EndGetResponse(result).GetResponseStream()))
        {
            server =  streamReader.ReadToEnd();
            print("AI request finished");
        }
    }
    private void Start()
    {
        enemies = new EnemyAIController[3];
        enemies = FindObjectsOfType<EnemyAIController>();
    }
    private void FixedUpdate()
    {
        if (!requestStatus)
        {
            //send req
            //print("AI request started");
            requestStatus = true;
            StartWebRequest();
        }
        else
        {
            if (server != "")
            {
                //print("sending");
                foreach (var c in enemies)
                {
                    c.ValueMatrix = ParseValueMatrix(c.Id);
                    c.stopped = false;
                }
                //print("sended");
                debugString = server;
                server = "";
                requestStatus = false;
            }
        }
        /*if (server != "")
        {
            if (!requestStatus)
            {
                print("sending");
                foreach(var c in enemies)
                {
                    c.ValueMatrix = ParseValueMatrix(c.Id);
                    c.stopped = false;
                }
                print("sended");
                requestStatus = true;
            }
        }*/
    }
    private double[,] ParseValueMatrix(int enemyId)
    {
        JObject obj = JObject.Parse(server);
        double[,] vm = new double[20, 20];
        int counter = 0;
        foreach (var p in obj.Property($"{enemyId}"))
        {
            foreach (var s in p)
            {
                foreach (var l in s)
                {
                    vm[counter / 20, counter % 20] = (double)l;
                    //print(vm[counter / 20, counter % 20]);
                    counter++;
                }
            }
        }
        //print(counter);
        return vm;
    }
    
    private string AIServerRequest()
    {
        var url = "http://pacman-npc-orchestrator.herokuapp.com/get_strategy";

        var httpRequest = (HttpWebRequest)WebRequest.Create(url);
        httpRequest.Method = "POST";

        httpRequest.ContentType = "application/json";

        var data = FindObjectOfType<LevelBuilder>().GetLevelMatrix().GetJsonString();
        using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
        {
            streamWriter.Write(data);
        }

        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            return streamReader.ReadToEnd();
        }
        /*using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "http://pacman-npc-orchestrator.herokuapp.com/get_strategy"))
            {
                request.Content = new StringContent("{\"level_matrix\": [[1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1], [1, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 1], [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1], [1, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1], [1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1], [1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1], [1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1], [1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 1], [1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1], [1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1], [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1], [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1], [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1], [1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 5, 0, 0, 1], [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1], [1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 1], [1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1]]}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                server = (await httpClient.SendAsync(request)).Content.ReadAsStringAsync();
            }
        }*/
    }
}
