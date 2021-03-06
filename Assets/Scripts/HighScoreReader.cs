using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
public class HighScoreReader : MonoBehaviour
{
    struct Stats
    {
        public string name;
        public int minutes;
        public int seconds;
    }
    
    string TABLE_PATH = "PlayerStatsTable.txt";
    string tableText = "";
    private List<Stats> stats;
    public GameObject textbox;
    void Start()
    {
        stats = new List<Stats>();
        ReadAndSortTable();
    }
    void ReadAndSortTable()
    {
        StreamReader reader = new StreamReader(TABLE_PATH, true);
        while (!reader.EndOfStream)
        {
            Stats stat;
            string statString = reader.ReadLine();
            var nametime = statString.Split(' ');
            stat.name = nametime[0];
            int minutes = (Mathf.RoundToInt(float.Parse(nametime[1]))) / 60;
            int seconds = (Mathf.RoundToInt(float.Parse(nametime[1]))) % 60;
            stat.minutes = minutes;
            stat.seconds = seconds;

            stats.Add(stat);
        }
        reader.Close();

        var sortedTable = from s in stats
                          orderby s.minutes, s.seconds descending
                          select s;

        stats = sortedTable.ToList();
        
        foreach(var s in stats)
        {
            string st = "";
            st = s.name + " " + s.minutes + ":" + s.seconds;
            tableText += st + "\n";
        }

        textbox.GetComponent<Text>().text = tableText;
    }
}
