using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class HighscoreTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    public List<Transform> highscoreEntryTransformList = new List<Transform>();
    //private List<HighscoreEntry> highscoreEntryList;
    //private List<HighscoreEntry> highscoreEntryList2;
    private PlacementManagement pm;
    public int currSel = 0;
    public int maxSel = 2;
    public Button nextButton;
    public Button prevButton;
    public Button backButton;
    public Button clearButton;
    public Button againButton;
    public TextMeshProUGUI selMap;

    public int n = 7;
    public int amount = 0;

    public void Awake()
    {
        pm = PlacementManagement.Instance;
        pm.CallOnAwake();

        selMap = GameObject.Find("MapNameText").GetComponent<TextMeshProUGUI>();
        nextButton = GameObject.Find("Next Button").GetComponent<Button>();
        prevButton = GameObject.Find("Prev Button").GetComponent<Button>();
        backButton = GameObject.Find("Back Button").GetComponent<Button>();
        clearButton = GameObject.Find("Clear Button").GetComponent<Button>();
        againButton = GameObject.Find("Again Button").GetComponent<Button>();

        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
        clearButton.onClick.RemoveAllListeners();
        againButton.onClick.RemoveAllListeners();

        nextButton.onClick.AddListener(() => pm.anim.SetTrigger("FlipN"));
        prevButton.onClick.AddListener(() => pm.anim.SetTrigger("FlipP"));
        nextButton.onClick.AddListener(() => AddSelection(true));
        prevButton.onClick.AddListener(() => AddSelection(false));
        againButton.onClick.AddListener(() => pm.StartGame());
        backButton.onClick.AddListener(GoToMenu);
        clearButton.onClick.AddListener(ClearList);

        if (File.Exists(Application.dataPath + "/highscoreEntry.json") == false)
        {
            Highscores highscores = new Highscores();
            highscores.highscoreEntryList0 = new List<HighscoreEntry>();
            highscores.highscoreEntryList1 = new List<HighscoreEntry>();
            highscores.highscoreEntryList2 = new List<HighscoreEntry>();

            string json = JsonUtility.ToJson(highscores);
            File.WriteAllText(Application.dataPath + "/highscoreEntry.json", json);
        }

        RefreshingHighscoreTable();
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

        float score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = pm.ConvertTimerInText(score);
        string name = highscoreEntry.name;

        entryTransform.Find("nameText").gameObject.GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    public void DeleteRankLine(List<Transform> transformList)
    {

        foreach (Transform t in transformList)
        {
            Destroy(t.gameObject);
        }

        transformList.Clear();
    }
   public void ClearList()
   {
        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        string jsonString = File.ReadAllText(Application.dataPath + "/highscoreEntry.json");

        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        switch (currSel)
        {
            case 0:

                highscores.highscoreEntryList0.Clear();
                break;
            case 1:

                highscores.highscoreEntryList1.Clear();
                break;
            case 2:

                highscores.highscoreEntryList2.Clear();

                break;
        }

      string json = JsonUtility.ToJson(highscores);
        //PlayerPrefs.SetString("highscoreTable", json);
        //PlayerPrefs.Save();

        File.WriteAllText(Application.dataPath + "/highscoreEntry.json", json);
        RefreshingHighscoreTable();

   }

    public void AddHighscoreEntry(float score, string name, int c)
    { 
        // Create Highscore Entry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name};

        // Load saved Highscores    
        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        string jsonString = File.ReadAllText(Application.dataPath + "/highscoreEntry.json");

        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        currSel = c;

        // Add new entry to Highscores
        switch (currSel)
        {
            case 0:

                highscores.highscoreEntryList0.Add(highscoreEntry);
                break;
            case 1:

                highscores.highscoreEntryList1.Add(highscoreEntry);
                break;
            case 2:

                highscores.highscoreEntryList2.Add(highscoreEntry);
                break;
        }


        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        //PlayerPrefs.SetString("highscoreTable", json);
        //PlayerPrefs.Save();
        File.WriteAllText(Application.dataPath + "/highscoreEntry.json", json);
    }

    public void RefreshingHighscoreTable()
    {       
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        string jsonString = File.ReadAllText(Application.dataPath + "/highscoreEntry.json");

        switch (currSel)
        {
            case 0:

                selMap.SetText(pm.map01Name);
                break;
            case 1:

                selMap.SetText(pm.map02Name);
                break;
            case 2:

                selMap.SetText(pm.map03Name);
                break;
        }

        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        DeleteRankLine(highscoreEntryTransformList);


        switch (currSel)
        {
            case 0:

                // Sort entry list by Score
                for (int i = 0; i < highscores.highscoreEntryList0.Count; i++)
                {
                    for (int j = i + 1; j < highscores.highscoreEntryList0.Count; j++)
                    {
                        if (highscores.highscoreEntryList0[j].score < highscores.highscoreEntryList0[i].score)  //i
                        {                                                                                       //j    
                                                                                                                // Swap
                            HighscoreEntry tmp = highscores.highscoreEntryList0[i];
                            highscores.highscoreEntryList0[i] = highscores.highscoreEntryList0[j];
                            highscores.highscoreEntryList0[j] = tmp;
                        }
                    }
                }

                foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList0)
                {
                    if (highscoreEntry.score > 0)
                    {
                        CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
                    }
                }
                break;
            case 1:

                // Sort entry list by Score
                for (int i = 0; i < highscores.highscoreEntryList1.Count; i++)
                {
                    for (int j = i + 1; j < highscores.highscoreEntryList1.Count; j++)
                    {
                        if (highscores.highscoreEntryList1[j].score < highscores.highscoreEntryList1[i].score)  //i
                        {                                                                                       //j    
                                                                                                                // Swap
                            HighscoreEntry tmp = highscores.highscoreEntryList1[i];
                            highscores.highscoreEntryList1[i] = highscores.highscoreEntryList1[j];
                            highscores.highscoreEntryList1[j] = tmp;
                        }
                    }
                }

                foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList1)
                {
                    if (highscoreEntry.score > 0)
                    {
                        CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
                    }
                }
                break;
            case 2:

                // Sort entry list by Score
                for (int i = 0; i < highscores.highscoreEntryList2.Count; i++)
                {
                    for (int j = i + 1; j < highscores.highscoreEntryList2.Count; j++)
                    {
                        if (highscores.highscoreEntryList2[j].score < highscores.highscoreEntryList2[i].score)  //i
                        {                                                                                       //j    
                                                                                                                // Swap
                            HighscoreEntry tmp = highscores.highscoreEntryList2[i];
                            highscores.highscoreEntryList2[i] = highscores.highscoreEntryList2[j];
                            highscores.highscoreEntryList2[j] = tmp;
                        }
                    }
                }

                foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList2)
                {
                    if (highscoreEntry.score > 0)
                    {
                        CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
                    }
                }
                break;
        }

    }

    public void AddSelection(bool d)
    {
        if (d)
        {
            currSel++;
            if (currSel > maxSel)
            {
                currSel = 0;
            }
        }
        else
        {
            currSel--;
            if (currSel < 0)
            {
                currSel = maxSel;
            }
        }

        RefreshingHighscoreTable();
    }

    public void GoToMenu()
    {
        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();
        clearButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        pm.BackToMenu();
    }    

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList0;
        public List<HighscoreEntry> highscoreEntryList1;
        public List<HighscoreEntry> highscoreEntryList2;
        
    }


    [System.Serializable]
    private class HighscoreEntry
    {
        public float score;
        public string name;
    }

}
