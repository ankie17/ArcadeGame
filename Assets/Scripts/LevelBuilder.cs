using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO; 
using UnityEngine;

[ExecuteInEditMode]
public class LevelBuilder : MonoBehaviour
{
    public enum LevelBuilderMode
    {
        Manual,
        Request
    }
    public LevelBuilderMode builderMode = LevelBuilderMode.Manual;
    public const int LEVEL_SIZE = 20;
    private int[,] levelMatrix;
    public string MatrixString = "";
    public GameObject wall;
    public GameObject floor;
    public GameObject star;
    public GameObject heart;
    private Vector3 startPos;
    void Start()
    {
        //получить от микросервиса строку с матрицей
        startPos = transform.position;
        levelMatrix = new int[LEVEL_SIZE, LEVEL_SIZE];
        //нужно вызвать метод парсинга и наполнить матрицу
        if (builderMode == LevelBuilderMode.Request)
        {
            LevelRequest();
        }
        //далее нужно проитерироваться по ней и построить уровень
        ParseMatrix();
        GeneratePickupCoordinates();
        BuildLevel();
    }
    private void FixedUpdate()
    {
        //Debug.Log(gameObject.transform.childCount);
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

    void LevelRequest()
    {
        var url = "https://pacman-level-generator.herokuapp.com/get_level";

        var httpRequest = (HttpWebRequest)WebRequest.Create(url);
        httpRequest.Method = "POST";

        httpRequest.ContentType = "application/x-www-form-urlencoded";

        var data = "level=easy";

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

        if (transform.childCount == 0)
        {
            for (int i = 0; i < LEVEL_SIZE; i++)
            {
                for (int j = 0; j < LEVEL_SIZE; j++)
                {
                    Vector3 currentPos = new Vector3(startPos.x + j, startPos.y - i, startPos.z);

                    if (levelMatrix[i, j] == 0)
                    {
                        var cell = Instantiate(floor, currentPos, Quaternion.identity);
                        cell.transform.parent = gameObject.transform;
                    }
                    else if (levelMatrix[i, j] == 1)
                    {
                        var cell = Instantiate(wall, currentPos, Quaternion.identity);
                        cell.transform.parent = gameObject.transform;
                    }
                    else if (levelMatrix[i, j] == 2)
                    {
                        //spawn star
                        var cell = Instantiate(star, currentPos, Quaternion.identity);
                        cell.transform.parent = gameObject.transform;
                        var v = Instantiate(floor, currentPos, Quaternion.identity);
                        v.transform.parent = gameObject.transform;
                    }
                    else if (levelMatrix[i, j] == 3)
                    {
                        //spawn heart
                        var heratCell = Instantiate(heart, currentPos, Quaternion.identity);
                        heratCell.transform.parent = gameObject.transform;
                        //heratCell.SetActive(false);
                        var v = Instantiate(floor, currentPos, Quaternion.identity);
                        v.transform.parent = gameObject.transform;
                    }
                }
            }
        }
    }
    void GeneratePickupCoordinates()
    {
        var rnd = new System.Random();
        List<int> lst = new List<int>();

        for(int i = 0; i < 4; i++)
        {
            //cгенерировать случайное число от 0 до 399
            int val = 0;
            int rowID = 0;
            int columnID = 0;
            do
            {
                val = rnd.Next(0, 400);
                rowID = val / LEVEL_SIZE;
                columnID = val % LEVEL_SIZE;
            } while (levelMatrix[rowID, columnID] == 1 || lst.Contains(val));
            //проверить является ли оно стеной или звездочкой
            lst.Add(val);
            
            //найти соответствующие координаты матрице уровня
            //и записать это число туда
        }
        for(int i = 0; i < 4; i++)
        {
            int val = lst[i];
            int rowID = val / LEVEL_SIZE;
            int columnID = val % LEVEL_SIZE;
            if (i == 3)
            {
                levelMatrix[rowID, columnID] = 3;
            }
            else
            {
                levelMatrix[rowID, columnID] = 2;
            }
        }
    }
}
