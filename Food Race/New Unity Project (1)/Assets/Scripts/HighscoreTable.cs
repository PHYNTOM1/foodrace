

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HighscoreTable : MonoBehaviour {

    public Transform entryContainer;
    public Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private void Awake() {

        RefreshHighscoreTable();
        //List<Transform> T2 = new List<Transform>();
        //Debug.Log(T2.Count);

        
    }

    public void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {

        
            float templateHeight = 31f;
            Transform entryTransform = Instantiate(entryTemplate, container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;
            string rankString;
            switch (rank) {
                default:
                    rankString = rank + "TH"; break;

                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;
            }
                entryTransform.Find("posText").GetComponent<Text>().text = rankString;

                float score = highscoreEntry.score;

                entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

                string name = highscoreEntry.name;
                entryTransform.Find("nameText").GetComponent<Text>().text = name;

        
        
        // Highlight First
        if (rank == 1) {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        

        transformList.Add(entryTransform);
        

    }


    public void AddHighscoreEntry(float score, string name) {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };
        
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        

        // Add new entry to Highscores
        highscores.highscoreEntryList2.Add(highscoreEntry);

        

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    public void RefreshHighscoreTable()
    {
        Debug.Log("Hat mich in den Arsch gebummst");
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);



        // Sort entry list by Score

        // if(highscores.highscoreEntryList1.Count != 0 && highscores.highscoreEntryList1.Count != null) { 
        for (int i = 0; i < highscores.highscoreEntryList2.Count; i++)
        {

            for (int j = i + 1; j > highscores.highscoreEntryList2.Count; j++)
            {

                if (highscores.highscoreEntryList2[j].score > highscores.highscoreEntryList2[i].score)
                {
                    // Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList2[i];
                    highscores.highscoreEntryList2[i] = highscores.highscoreEntryList2[j];
                    highscores.highscoreEntryList2[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList2)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    //}
    }

    /*public void ClearList()
    {
        //Debug.Log("Hat mich in den Arsch gebummst");
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
        highscores.highscoreEntryList1.Clear();

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

        RefreshHighscoreTable();

        PlacementManagement.Instance.LoadEndscreenScene(false);


    }*/

    public class Highscores { 
        
        public List<HighscoreEntry> highscoreEntryList2;
    }

    /*
     * Represents a single High score entry
     * */
    [System.Serializable] 
    public class HighscoreEntry {
        public float score;
        public string name;
    }

}
