using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

public class ResourcesLoader : MonoBehaviour
{
    HttpWebRequest webRequest;
    private string response;
    private bool received = false;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartTexturesRequest();
    }
    private void Update()
    {
        if (received)
        {
            received = false;
            ParseDataAndCreateTextures();
        }
    }
    void StartTexturesRequest()
    {
        var url = "http://pacman-texture-generator.herokuapp.com/get_textures_png?n_bricks=32&n_roads=32";

        webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.Method = "GET";

        webRequest.ContentType = "application/x-www-form-urlencoded";
        webRequest.BeginGetResponse(new AsyncCallback(FinishTexturesRequest), null);
    }
    void SaveTexture(string s, int width, int height, int counter, string name)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(Convert.FromBase64String(s));

        var data = texture.EncodeToPNG();
        File.WriteAllBytes($"Textures\\{name}{counter}.png", data);
    }
    void FinishTexturesRequest(IAsyncResult result)
    {
        using (var streamReader = new StreamReader(webRequest.EndGetResponse(result).GetResponseStream()))
        {
            response = streamReader.ReadToEnd();
        }
        received = true;
    }
    void ParseDataAndCreateTextures()
    {
        var obj = JObject.Parse(response);
        foreach (var p in obj.Property("bricks"))
        {
            int i = 0;
            foreach (var s in p)
            {
                string base64 = ((string)s).Trim('\n');
                SaveTexture(base64, 28, 14, i, "brick");
                i++;
            }
        }
        foreach (var p in obj.Property("roads"))
        {
            int i = 0;
            foreach (var s in p)
            {
                string base64 = ((string)s).Trim('\n');
                SaveTexture(base64, 28, 28, i, "grass");
                i++;
            }
        }
        Destroy(gameObject);
    }
}