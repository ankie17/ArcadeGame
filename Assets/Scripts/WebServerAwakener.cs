using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WebServerAwakener : MonoBehaviour
{
    void Start()
    {
        AwakeLevelGenerator();
        AwakeNpcOrchestrator();
        AwakeTextureGenerator();
        SceneManager.LoadScene("MainMenu");
    }
    void AwakeLevelGenerator()
    {
        var url = "http://pacman-level-generator.herokuapp.com/";

        var httpRequest = (HttpWebRequest)WebRequest.Create(url);
        httpRequest.Method = "GET";

        httpRequest.ContentType = "application/x-www-form-urlencoded";

        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
        }
    }
    void AwakeTextureGenerator()
    {
        var url = "http://pacman-texture-generator.herokuapp.com/";
        var httpRequest = (HttpWebRequest)WebRequest.Create(url);
        httpRequest.Method = "GET";

        httpRequest.ContentType = "application/x-www-form-urlencoded";

        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
        }
    }
    void AwakeNpcOrchestrator()
    {
        var url = "http://pacman-npc-orchestrator.herokuapp.com/";
        var httpRequest = (HttpWebRequest)WebRequest.Create(url);
        httpRequest.Method = "GET";

        httpRequest.ContentType = "application/x-www-form-urlencoded";

        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
        }
    }
}
