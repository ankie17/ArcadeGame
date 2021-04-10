using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

public class ResourcesLoader : MonoBehaviour
{
    // Start is called before the first frame update
    /*void Start()
    {
        int n = 32;
        var obj = JObject.Parse(Request(n));
        var bricks = new List<Brick>();
        for (int count = 0; count < n; count++)
        {
            bricks.Add(new Brick());
        }
        int[] nums = new int[n * 3 * 14 * 28];
        int i = 0;
        foreach (var p in obj.Property("bricks"))
        {
            foreach (var s in p)
            {
                foreach (var l in s)
                {
                    foreach (var z in l)
                    {
                        foreach (var u in z)
                        {
                            nums[i++] = (int)u;
                        }
                    }
                }
            }
        }
        for (int j = 0; j < n; j++)
        {
            int[] brick = new int[3 * 14 * 28];
            System.Array.Copy(nums, j * 3 * 14 * 28, brick, 0, 3 * 14 * 28);
            bricks[j].Parse(brick);
            bricks[j].Convert(j);
        }
    }
    static string Request(int n)
    {
        var url = "http://pacman-texture-generator.herokuapp.com/get_textures?n_bricks=";
        url += n.ToString();
        var httpRequest = (HttpWebRequest)WebRequest.Create(url);
        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            return streamReader.ReadToEnd();
        }
    }*/
}
