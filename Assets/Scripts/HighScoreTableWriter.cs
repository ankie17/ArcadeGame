using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScoreTableWriter : MonoBehaviour
{
    string TABLE_PATH = "PlayerStatsTable.txt";
    public void WritePlayerStats(string playerName, float levelTime)
    {
        //в конец файла записать статистику игрока
        StreamWriter writer = new StreamWriter(TABLE_PATH, true);
        
        writer.WriteLine(playerName + " " + levelTime);

        writer.Close();
    }
}
