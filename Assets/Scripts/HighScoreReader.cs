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

    private const string CAMPAIGN_PATH = "Tables\\storyline.txt";
    private const string EASY_PATH = "Tables\\easy.txt";
    private const string MEDIUM_PATH = "Tables\\medium.txt";
    private const string HARD_PATH = "Tables\\hard.txt";

    private List<Stats> stats = new List<Stats>();
    [SerializeField]
    private Text easyTextBox;
    [SerializeField]
    private Text mediumTextBox;
    [SerializeField]
    private Text hardTextBox;
    [SerializeField]
    private Text campaignTextBox;
    private void Start()
    {
        easyTextBox.text = ReadSortAndWriteTable(1);
        mediumTextBox.text = ReadSortAndWriteTable(2);
        hardTextBox.text = ReadSortAndWriteTable(3);

        campaignTextBox.text = ReadCampaignTable();
    }
    private string ReadCampaignTable()
    {
        string result = "";
        StreamReader reader = new StreamReader(CAMPAIGN_PATH, true);
        while (!reader.EndOfStream)
        {
            result += reader.ReadLine() + "\n";
        }
            return result;
    }
    private string ReadSortAndWriteTable(int id)
    {
        string result = "";
        string path;

        if (id == 1)
            path = EASY_PATH;
        else if (id == 2)
            path = MEDIUM_PATH;
        else
            path = HARD_PATH;

        StreamReader reader = new StreamReader(path, true);
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
                          orderby s.minutes, s.seconds ascending
                          select s;

        stats = sortedTable.ToList();

        foreach (var s in stats)
        {
            string st = "";
            st = s.name + " " + s.minutes + "m " + s.seconds + "s";
            result += st + "\n";
        }

        stats = new List<Stats>();

        return result;
    }
}
