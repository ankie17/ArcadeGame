using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO; 
using UnityEngine;
using System;

public enum LevelDifficulty
{
    easy,
    medium,
    hard
}
public class LevelBuilder : MonoBehaviour
{
    enum LevelBuilderMode
    {
        Manual,
        Request
    }
    [SerializeField]
    private GrassLoader grassLoader;
    public LevelDifficulty difficulty;
    [SerializeField]
    private LevelBuilderMode builderMode = LevelBuilderMode.Manual;
    private int[,] levelMatrix;
    [SerializeField]
    private string MatrixString = "";
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject floorPrefab;
    [SerializeField]
    private GameObject starPrefab;
    [SerializeField]
    private GameObject heartPrefab;
    [SerializeField]
    private GameObject playerPrefab;
    private Vector3 startPos;

    #region constants
    private const int LEVEL_SIZE = 20;
    const int floorID = 0;
    const int wallID = 1;
    const int starID = 6;
    const int heartID = 7;
    #endregion
    public void Prepare()
    {
        if (builderMode == LevelBuilderMode.Request)
        {
            GenerateLevel();
        }
    }
    [ContextMenu("GenerateLevel")]
    public void GenerateLevel()
    {
        startPos = transform.position;
        GenerateMatrix();
        grassLoader.LoadGrass();
        BuildLevel();
        SpawnPlayer();
        FindObjectOfType<AIManager>().SpawnEnemies();
        GameManager.Instance.FindAndAssignKeyGameObjects();
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
    public LevelMatrix GetLevelMatrix()
    {
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
    private void ParseMatrix()
    {
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

        var data = "level="+difficulty.ToString();

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
                        var cell = Instantiate(floorPrefab, currentPos, Quaternion.identity);
                        //set grass sprite
                        cell.GetComponent<SpriteRenderer>().sprite = grassLoader.GetGrassSprite();
                        cell.transform.parent = gameObject.transform;
                    }
                    else if (levelMatrix[i, j] == wallID)
                    {
                        var cell = Instantiate(wallPrefab, currentPos, Quaternion.identity);
                        cell.transform.parent = gameObject.transform;
                    }
                    else if (levelMatrix[i, j] == starID)
                    {
                        //spawn star
                        var cell = Instantiate(starPrefab, currentPos, Quaternion.identity);
                        cell.transform.parent = gameObject.transform;
                        
                        var v = Instantiate(floorPrefab, currentPos, Quaternion.identity);
                        v.transform.parent = gameObject.transform;
                        v.GetComponent<SpriteRenderer>().sprite = grassLoader.GetGrassSprite();
                    }
                    else if (levelMatrix[i, j] == heartID)
                    {
                        //spawn heart
                        var heratCell = Instantiate(heartPrefab, currentPos, Quaternion.identity);
                        heratCell.transform.parent = gameObject.transform;
                        
                        //heratCell.SetActive(false);
                        var v = Instantiate(floorPrefab, currentPos, Quaternion.identity);
                        v.transform.parent = gameObject.transform;
                        v.GetComponent<SpriteRenderer>().sprite = grassLoader.GetGrassSprite();
                    }
                }
            }
        }
    }
    void GeneratePickupCoordinates()
    {
        int[][] starPositions = new int[3][];
        starPositions[0] = GenerateRandomCoordinateInRange(11, 17, 2, 9);
        starPositions[1] = GenerateRandomCoordinateInRange(11, 17, 11, 17);
        starPositions[2] = GenerateRandomCoordinateInRange(2, 9, 2, 9);
        var heartPos = GenerateRandomCoordinateInRange(2, 17, 2, 17);

        foreach(var sp in starPositions)
        {
            levelMatrix[sp[0], sp[1]] = starID;
        }

        levelMatrix[heartPos[0], heartPos[1]] = heartID;
    }
    int[] GenerateRandomCoordinateInRange(int minX, int maxX, int minY, int maxY)
    {
        while (true)
        {
            
            int posX = UnityEngine.Random.Range(minX, maxX + 1);
            int posY = UnityEngine.Random.Range(minY, maxY + 1);
            if (levelMatrix[posY, posX] == 0)
            {
                int[] result = { posY, posX };
                return result;
            }
        }
    }
    void SpawnPlayer()
    {
        var playerCoord = GenerateRandomCoordinateInRange(2, 6, 12, 16);
        Vector3 playerPos = new Vector3(playerCoord[1], MirrorYByMiddle(playerCoord[0]), 0);
        var player = Instantiate(playerPrefab, playerPos, Quaternion.identity);
        if (player == null)
        GameManager.Instance.player = player;
    }
}
