using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScoreTableWriter : MonoBehaviour
{
    private const string CAMPAIGN_PATH = "Tables\\storyline.txt";
    private const string EASY_PATH = "Tables\\easy.txt";
    private const string MEDIUM_PATH = "Tables\\medium.txt";
    private const string HARD_PATH = "Tables\\hard.txt";
    public void WritePlayerStats(string playerName, int tableID, int levelTime)
    {
        string path;
        string logLine = playerName + " " + levelTime;

        if (tableID == 0)
        {
            path = CAMPAIGN_PATH;
            logLine = playerName;
        }
        else if (tableID == 1)
        {
            path = EASY_PATH;
        }
        else if (tableID == 2)
            path = MEDIUM_PATH;
        else
            path = HARD_PATH;

        StreamWriter writer = new StreamWriter(path, true);
        
        writer.WriteLine(logLine);

        writer.Close();
    }
}
