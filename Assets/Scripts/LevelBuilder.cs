using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO; 
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public const int LEVEL_SIZE = 20;
    private int[,] levelMatrix;
    public string MatrixString = "";
    public GameObject wall;
    public GameObject floor;
    private Vector3 startPos;
    void Start()
    {
        //получить от микросервиса строку с матрицей
        startPos = transform.position;
        levelMatrix = new int[LEVEL_SIZE, LEVEL_SIZE];
        //нужно вызвать метод парсинга и наполнить матрицу
        LevelRequest();
        //далее нужно проитерироваться по ней и построить уровень
        ParseMatrix();
        BuildLevel();
    }
    public void ParseMatrix()
    {
        //нужно пропарсить стринг
        int counter = 0;
        foreach (char c in MatrixString)
        {
            if (c == '0' || c == '1')
            {
                int rowID = counter / LEVEL_SIZE;
                int columnID = counter % LEVEL_SIZE;
                int number = int.Parse(c.ToString());
                levelMatrix[rowID, columnID] = number;
                counter++;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void LevelRequest()
    {
        var url = "http://127.0.0.1:5000/get_level";

        var httpRequest = (HttpWebRequest)WebRequest.Create(url);
        httpRequest.Method = "POST";

        httpRequest.ContentType = "application/x-www-form-urlencoded";

        var data = "level=hard";

        using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
        {
            streamWriter.Write(data);
        }

        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            MatrixString = result;
        }
    }
    void BuildLevel()
    {
        for (int i=0; i<LEVEL_SIZE; i++)
        {
            for (int j = 0; j < LEVEL_SIZE; j++)
            {
                Vector3 currentPos = new Vector3(startPos.x+j, startPos.y-i, startPos.z);

                if (levelMatrix[i, j] == 0)
                {
                    Instantiate(floor, currentPos, Quaternion.identity);
                }
                else if (levelMatrix[i, j] == 1)
                {
                    Instantiate(wall, currentPos, Quaternion.identity);
                }
            }
        }
    }
}
