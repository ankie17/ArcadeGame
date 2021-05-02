using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMatrix
{
    public int[][] level_matrix;
    public LevelMatrix()
    {
        level_matrix = new int[20][];
        for(int i = 0; i < 20; i++)
        {
            level_matrix[i] = new int[20];
        }
    }
    public string GetJsonString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
