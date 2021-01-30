using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HighscoreTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    public List<Transform> highscoreEntryTransformList;
    //private List<HighscoreEntry> highscoreEntryList;
    private List<HighscoreEntry> highscoreEntryList2;


    public int n = 7;




    public int amount = 0;

    public void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        AddHighscoreEntry(5, "");
        AddHighscoreEntry(1200, "");
        AddHighscoreEntry(9000, "");
        AddHighscoreEntry(1100, "");
        


        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
       

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList2)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

    }
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {

        float templateHeight = 35f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);


        int rank = transformList.Count + 1;
        string rankString;


        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }



        entryTransform.Find("posText").gameObject.GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;

        entryTransform.Find("nameText").gameObject.GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int  score, string name)
    {
        // Create Highscore Entry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name};
        
        // Load saved Highscores    
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);



        // Add new entry to Highscores
        highscores.highscoreEntryList2.Add(highscoreEntry);

        for (int i = 0; i < highscores.highscoreEntryList2.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList2.Count; j++)
            {
                if (highscores.highscoreEntryList2[j].score < highscores.highscoreEntryList2[i].score)
                {
                    HighscoreEntry tmp = highscores.highscoreEntryList2[i];
                    highscores.highscoreEntryList2[i] = highscores.highscoreEntryList2[j];
                    highscores.highscoreEntryList2[j] = tmp;
                }
            }
        }


        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();




    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
        public List<HighscoreEntry> highscoreEntryList2;
    }


    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }

}
