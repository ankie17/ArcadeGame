using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO; 
using UnityEngine;
using System;

[ExecuteInEditMode]
public class LevelBuilder : MonoBehaviour
{
    public enum LevelBuilderMode
    {
        Manual,
        Request
    }
    public GrassLoader grassLoader;
    public LevelBuilderMode builderMode = LevelBuilderMode.Manual;
    public const int LEVEL_SIZE = 20;
    private int[,] levelMatrix;
    public string MatrixString = "";
    public GameObject wall;
    public GameObject floor;
    public GameObject star;
    public GameObject heart;
    private Vector3 startPos;

    #region constants
    /*
     * 0-пол
1-стена
2-игрок
3-4-5 -враги
6-звезды
7-сердечко
     */
    const int floorID = 0;
    const int wallID = 1;
    const int starID = 6;
    const int heartID = 7;
    #endregion

    [ContextMenu("GenerateLevel")]
    public void GenerateLevel()
    {
        //получить от микросервиса строку с матрицей
        startPos = transform.position;
        GenerateMatrix();
        BuildLevel();
    }
    private void GenerateMatrix()
    {
        levelMatrix = new int[LEVEL_SIZE, LEVEL_SIZE];
        if (builderMode == LevelBuilderMode.Request)
        {
            LevelRequest();
        }
        ParseMatrix();
        GeneratePickupCoordinates();
    }
    [ContextMenu("DestroyChilds")]
    public void DestroyChilds()
    {
        foreach (Transform c in transform)
        {
            DestroyImmediate(c.gameObject);
        }
    }
    private void FixedUpdate()
    {
        //Debug.Log(gameObject.transform.childCount);
    }
    /*public LevelMatrix GetLevelMatrix()
    {
        GenerateMatrix();

        int[][] m = new int[20][];
        for (int i = 0; i < 20; i++)
        {
            m[i] = new int[20];
            for (int j = 0; j < 20; j++)
            {
                int id = levelMatrix[i, j];
                if (id==6||id==7) //delete old pickup coords, we will find it later
                    id = 0;
                m[i][j] = id;

            }
        }
        //find pickup coords
        var stars = FindObjectsOfType<Star>();
        if (stars.Length > 0)//if they exist
        {
            //write to matrix
            foreach (var s in stars)
            {
                var pos = s.GetStarPos();
                int x = (int)pos.x;
                int y = MirrorYByMiddle((int)pos.y);
                m[y][x] = 6;
            }
        }

        //find heart coords
        var heart = FindObjectOfType<Heart>();
        if (heart != null)//if it exists
        {
            //write to matrix
            var pos = heart.GetHeartPos();
            int x = (int)pos.x;
            int y = MirrorYByMiddle((int)pos.y);
            m[y][x] = 7;
        }
        
        //find enemies and write their coords
        foreach(var e in FindObjectsOfType<EnemyAIController>())
        {
            m[e.posY][e.posX] = e.Id;
        }

        //find player and write his coords
        Vector2 playerPos = FindObjectOfType<PlayerController>().GetPlayerPos();
        m[MirrorYByMiddle((int)playerPos.y)][(int)playerPos.x] = 2; //player ID=2

        //create level matrix object and write to it
        LevelMatrix lm = new LevelMatrix();
        lm.level_matrix = m;
        return lm;
    }*/
    public LevelMatrix GetLevelMatrix()
    {
        GenerateMatrix();

        int[][] m = new int[20][];
        for (int i = 0; i < 20; i++)
        {
            m[i] = new int[20];
            for (int j = 0; j < 20; j++)
            {
                int id = levelMatrix[i, j];
                if (id == 6 || id == 7) //delete old pickup coords, we will find it later
                    id = 0;
                m[i][j] = id;

            }
        }
        //coord - string value dictionary
        Dictionary<Tuple<int, int>, string> coordId = new Dictionary<Tuple<int, int>, string>();

        //find player and write his coords
        Vector2 playerPos = FindObjectOfType<PlayerController>().GetPlayerPos();
        Tuple<int, int> keyPlayer = new Tuple<int, int>(MirrorYByMiddle((int)playerPos.y), (int)playerPos.x);
        coordId[keyPlayer] = "";
        coordId[keyPlayer] += "2";

        //find enemies and write their coords
        var enemies = FindObjectsOfType<EnemyAIController>();
        enemies = (from e in enemies
                   orderby e.Id ascending
                   select e).ToArray();

        foreach(var e in enemies)
        {
            Tuple<int, int> keyEnemy = new Tuple<int, int>(e.posY, e.posX);
            if(!coordId.ContainsKey(keyEnemy))
                coordId[keyEnemy] = "";
        }
        foreach (var e in enemies)
        {
            Tuple<int, int> keyEnemy = new Tuple<int, int>(e.posY, e.posX);
            coordId[keyEnemy] += e.Id.ToString();
        }

        //find star coords
        var stars = FindObjectsOfType<Star>();
        if (stars.Length > 0)//if they exist
        {
            foreach(var s in stars)
            {
                var pos = s.GetStarPos();
                int x = (int)pos.x;
                int y = MirrorYByMiddle((int)pos.y);

                Tuple<int, int> keyStar = new Tuple<int, int>(y, x);
                if (!coordId.ContainsKey(keyStar))
                    coordId[keyStar] = "";
            }
            //write to matrix
            foreach (var s in stars)
            {
                var pos = s.GetStarPos();
                int x = (int)pos.x;
                int y = MirrorYByMiddle((int)pos.y);

                Tuple<int, int> keyStar = new Tuple<int, int>(y, x);
                coordId[keyStar] += "6";
            }
        }

        //find heart coords
        var heart = FindObjectOfType<Heart>();
        if (heart != null)//if it exists
        {
            //write to matrix
            var pos = heart.GetHeartPos();
            int x = (int)pos.x;
            int y = MirrorYByMiddle((int)pos.y);

            Tuple<int, int> keyHeart = new Tuple<int, int>(y, x);
            if (!coordId.ContainsKey(keyHeart))
                coordId[keyHeart] = "";
            coordId[keyHeart] += "7";
        }
        //write to matrix
        foreach(var pair in coordId)
        {
            int x = pair.Key.Item2;
            int y = pair.Key.Item1;

            int id = int.Parse(pair.Value);

            m[y][x] = id;
        }
        //create level matrix object and write to it
        LevelMatrix lm = new LevelMatrix();
        lm.level_matrix = m;
        return lm;
    }
    private int MirrorYByMiddle(int y)
    {
        int mirrored = 19 - y;
        return mirrored;
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

                    if (levelMatrix[i, j] == floorID)
                    {
                        var cell = Instantiate(floor, currentPos, Quaternion.identity);
                        //set grass sprite
                        cell.GetComponent<SpriteRenderer>().sprite = grassLoader.GetGrassSprite();
                        cell.transform.parent = gameObject.transform;
                    }
                    else if (levelMatrix[i, j] == wallID)
                    {
                        var cell = Instantiate(wall, currentPos, Quaternion.identity);
                        cell.transform.parent = gameObject.transform;
                    }
                    else if (levelMatrix[i, j] == starID)
                    {
                        //spawn star
                        var cell = Instantiate(star, currentPos, Quaternion.identity);
                        cell.transform.parent = gameObject.transform;
                        
                        var v = Instantiate(floor, currentPos, Quaternion.identity);
                        v.transform.parent = gameObject.transform;
                        v.GetComponent<SpriteRenderer>().sprite = grassLoader.GetGrassSprite();
                    }
                    else if (levelMatrix[i, j] == heartID)
                    {
                        //spawn heart
                        var heratCell = Instantiate(heart, currentPos, Quaternion.identity);
                        heratCell.transform.parent = gameObject.transform;
                        
                        //heratCell.SetActive(false);
                        var v = Instantiate(floor, currentPos, Quaternion.identity);
                        v.transform.parent = gameObject.transform;
                        v.GetComponent<SpriteRenderer>().sprite = grassLoader.GetGrassSprite();
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
                levelMatrix[rowID, columnID] = heartID;
            }
            else
            {
                levelMatrix[rowID, columnID] = starID;
            }
        }
    }
}
