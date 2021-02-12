using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms.Impl;
using System.Net;

public class PlacementManagement : MonoBehaviour
{
    public List<GameObject> racers;
    public List<GameObject> finishers;
    public float bestTimeOfAll;
    public GameObject bestRacer;
    public bool finished = false;
    public bool loadingAwake = false;
    public int selectedMap = 0;

    public HighscoreTable he;

    public string LoadingScreen;
    public string MainMenu;
    public string Ingame2;

    public string map01Name, map02Name, map03Name;

    public Button backButton, clearButton, scoreButton;
    public GameObject mainMenu, optionsMenu, creditsMenu, mapMenu;
    public Button startButton, creditsButton, optionsButton;
    public Button obButton, cbButton, mbButton;

    public GameObject bgCanvas;
    public Animator anim;
    public CanvasGroup cg;

    //Slider progressBar;
    AsyncOperation loadingOperation;

    private static PlacementManagement _instance;
    public static PlacementManagement Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
    }

    void Start()
    {
        if (bgCanvas == null)
        {
            bgCanvas = GameObject.Find("BGCanvas");            
        }
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        if (cg == null)
        {
            cg = bgCanvas.GetComponentInChildren<CanvasGroup>();
        }
    }

    public void Anim1()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (mainMenu == null)
            {
                mainMenu = FindObjectOfType<MainMenu>().gameObject;                
            }
            if (optionsMenu == null)
            {
                optionsMenu = GameObject.Find("OptionsMenu");
            }
            if (creditsMenu == null)
            {
                creditsMenu = GameObject.Find("CreditsMenu");
            }
            if (mapMenu == null)
            {
                mapMenu = GameObject.Find("MapMenu");
            }

            Button[] _b = mainMenu.GetComponentsInChildren<Button>();
            foreach (Button b in _b)
            {
                if (b.gameObject.name == "StartButton")
                {
                    startButton = b;
                }
                else if (b.gameObject.name == "CreditsButton")
                {
                    creditsButton = b;
                }
                else if (b.gameObject.name == "OptionsButton")
                {
                    optionsButton = b;
                }
            }

            Button[] _b2 = mapMenu.GetComponentsInChildren<Button>();
            foreach (Button b in _b2)
            {
                if (b.gameObject.name == "MapBackButton")
                {
                    mbButton = b;
                }
            }

            obButton = optionsMenu.GetComponentInChildren<Button>();
            cbButton = creditsMenu.GetComponentInChildren<Button>();

            startButton.onClick.RemoveAllListeners();
            creditsButton.onClick.RemoveAllListeners();
            optionsButton.onClick.RemoveAllListeners();
            obButton.onClick.RemoveAllListeners();
            cbButton.onClick.RemoveAllListeners();
            mbButton.onClick.RemoveAllListeners();

            startButton.onClick.AddListener(() => anim.SetTrigger("FlipN"));
            creditsButton.onClick.AddListener(() => anim.SetTrigger("FlipN"));
            optionsButton.onClick.AddListener(() => anim.SetTrigger("FlipN"));
            obButton.onClick.AddListener(() => anim.SetTrigger("FlipP"));
            cbButton.onClick.AddListener(() => anim.SetTrigger("FlipP"));
            mbButton.onClick.AddListener(() => anim.SetTrigger("FlipP"));

            startButton.onClick.AddListener(() => mapMenu.SetActive(true));
            startButton.onClick.AddListener(() => mainMenu.SetActive(false));
            creditsButton.onClick.AddListener(() => creditsMenu.SetActive(true));
            creditsButton.onClick.AddListener(() => mainMenu.SetActive(false));
            optionsButton.onClick.AddListener(() => optionsMenu.SetActive(true));
            optionsButton.onClick.AddListener(() => mainMenu.SetActive(false));
            obButton.onClick.AddListener(() => mainMenu.SetActive(true));
            obButton.onClick.AddListener(() => optionsMenu.SetActive(false));
            cbButton.onClick.AddListener(() => mainMenu.SetActive(true));
            cbButton.onClick.AddListener(() => creditsMenu.SetActive(false));
            mbButton.onClick.AddListener(() => mainMenu.SetActive(true));
            mbButton.onClick.AddListener(() => mapMenu.SetActive(false));

            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            creditsMenu.SetActive(false);
            mapMenu.SetActive(false);

            SetObjCanvas("MainMenuCanvas", false);
            bestTimeOfAll = 0f;
        }
        else if (SceneManager.GetActiveScene().name == "ScoreScreen")
        {
            if (he == null)
            {
                he = (HighscoreTable)FindObjectOfType(typeof(HighscoreTable));
            }

            if (bestTimeOfAll > 0f)
            {
                he.AddHighscoreEntry(bestTimeOfAll, "", selectedMap);
                bestTimeOfAll = 0f;
            }
            he.RefreshingHighscoreTable();
            SetObjCanvas("ScoreCanvas", false);
        }
    }

    public void Anim2()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SetObjCanvas("MainMenuCanvas", true);

            scoreButton = GameObject.Find("ScoreButton").GetComponentInChildren<Button>();
            scoreButton.onClick.AddListener(GoToScore);
        }
        else if (SceneManager.GetActiveScene().name == "ScoreScreen")
        {
            SetObjCanvas("ScoreCanvas", true);
        }
    }

    public void Anim3()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SetObjCanvas("MainMenuCanvas", false);
        }
        else if (SceneManager.GetActiveScene().name == "ScoreScreen")
        {
            SetObjCanvas("ScoreCanvas", false);
        }
    }

    public void Anim4()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            LoadScoreScreen();
        }
        else if (SceneManager.GetActiveScene().name == "ScoreScreen")
        {
            LoadMainMenu();
        }
        else if (SceneManager.GetActiveScene().name == "Ingame2")
        {
            if (finished == true)
            {
                LoadScoreScreen();
            }
            else
            {
                LoadMainMenu();
            }
        }
    }

    public void Anim5()
    {
        if (SceneManager.GetActiveScene().name == "Ingame2")
        {
            if (cg.alpha == 0)
            {
                cg.alpha = 1;
            }
            SetObjCanvas("Canvas", false);
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SetObjCanvas("MapCanvas", false);
        }
    }

    public void Flip1()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (mainMenu.activeInHierarchy)
            {
                if (GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled == true)
                {
                    SetObjCanvas("MainMenuCanvas", false);
                }
                else if (GameObject.Find("MainMenuCanvas").GetComponent<Canvas>().enabled == false)
                {
                    SetObjCanvas("MainMenuCanvas", true);
                }
            }
            else if (optionsMenu.activeInHierarchy)
            {
                if (GameObject.Find("OptionsCanvas").GetComponent<Canvas>().enabled == true)
                {
                    SetObjCanvas("OptionsCanvas", false);
                }
                else if (GameObject.Find("OptionsCanvas").GetComponent<Canvas>().enabled == false)
                {
                    SetObjCanvas("OptionsCanvas", true);
                }
            }
            else if (creditsMenu.activeInHierarchy)
            {
                if (GameObject.Find("CreditsCanvas").GetComponent<Canvas>().enabled == true)
                {
                    SetObjCanvas("CreditsCanvas", false);
                }
                else if (GameObject.Find("CreditsCanvas").GetComponent<Canvas>().enabled == false)
                {
                    SetObjCanvas("CreditsCanvas", true);
                }
            }
            else if (mapMenu.activeInHierarchy)
            {
                if (GameObject.Find("MapCanvas").GetComponent<Canvas>().enabled == true)
                {
                    SetObjCanvas("MapCanvas", false);
                }
                else if (GameObject.Find("MapCanvas").GetComponent<Canvas>().enabled == false)
                {
                    SetObjCanvas("MapCanvas", true);
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "ScoreScreen")
        {
            if (GameObject.Find("ScoreCanvas").GetComponent<Canvas>().enabled == true)
            {
                SetObjCanvas("ScoreCanvas", false);
            }
            else if (GameObject.Find("ScoreCanvas").GetComponent<Canvas>().enabled == false)
            {
                SetObjCanvas("ScoreCanvas", true);
            }
        }
    }


    private void SetObjCanvas(string s, bool b)
    {
        GameObject.Find(s).GetComponent<Canvas>().enabled = b;
    }

    public void StartGame()
    {
        anim.SetTrigger("IngameI");
        //loadingOperation = SceneManager.LoadSceneAsync("LoadingScreen");
    }

    public void GoToScore()
    {
        LoadEndscreenScene(false);
    }    

    public void BackToMenu()
    {
        if (SceneManager.GetActiveScene().name == "Ingame2")
        {
            anim.SetTrigger("IngameO");
        }
        else if (SceneManager.GetActiveScene().name == "ScoreScreen")
        {
            anim.SetTrigger("SceneO");
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLoadingScreen()
    {
        SceneManager.LoadScene("LoadingScreen");
    }

    public void LoadScoreScreen()
    {
        SceneManager.LoadScene("ScoreScreen");
    }

    public void CallOnAwake()
    {
        if (bgCanvas == null)
        {
            bgCanvas = GameObject.Find("BGCanvas");
        }
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        if (cg == null)
        {
            cg = bgCanvas.GetComponentInChildren<CanvasGroup>();
        }
        
        Scene aScene = SceneManager.GetActiveScene();
        Debug.Log("NEW SCENE LOADED: " + aScene.name);

        if (aScene.name == "MainMenu" || aScene.name == "ScoreScreen")
        {
            cg.alpha = 1;
            anim.SetTrigger("SceneI");
            loadingAwake = false;
        }
        else if (aScene.name == "LoadingScreen")
        {
            cg.alpha = 0;
            loadingOperation = SceneManager.LoadSceneAsync("Ingame2");
        }
        else if (aScene.name == "Ingame2")
        {
            cg.alpha = 0;
            loadingAwake = false;

            bool one = false;
            bool two = false;
            bool three = false;

            switch (selectedMap)
            {        
                case 0:

                    one = true;
                    break;
                case 1:

                    two = true;
                    break;
                case 2:

                    three = true;
                    break;
            }

            GameObject m01 = GameObject.FindGameObjectWithTag("Map01");
            GameObject m02 = GameObject.FindGameObjectWithTag("Map02");
            GameObject m03 = GameObject.FindGameObjectWithTag("Map03");
            m01.SetActive(one);
            m02.SetActive(two);
            m03.SetActive(three);

            racers.Clear();
            racers.Add(GameObject.FindGameObjectWithTag("Player"));
            bestRacer = racers[0];
            bestTimeOfAll = 0f;
            finishers.Clear();
            bestRacer.GetComponent<RoundTimer>().SetResetTimes(selectedMap);

            /*
        if (finishers.Count > 0)
        {
            for (int i = 0; i < finishers.Count; i++)
            {
                Destroy(finishers[i].gameObject);
            }
        }
            */
            //            RefreshRacers();            
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "LoadingScreen")
        {
            if (loadingAwake == false)
            {
                CallOnAwake();
                loadingAwake = true;
            }

            /*
            if (progressBar == null)
            {
                progressBar = FindObjectOfType<Slider>();

            }
            progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            */

            /*
            if (loadingOperation.isDone)
            {
                SceneManager.LoadScene("Ingame2");
            }
            */
        }              

        /*
          if (SceneManager.GetActiveScene().name == "Ingame2")
          {
              //UpdatePlacements();
                          if (finishers.Count == racers.Count)
                          {
                              LoadEndscreenScene();
                          }
          }
              */
    }

    public void GetBestTimeOverall()
    {
        bestTimeOfAll = 0f;

        if (finishers.Count > 0)
        {
            RoundTimer rt = finishers[0].gameObject.GetComponent<RoundTimer>();

            if (rt != null)
            {
                bestTimeOfAll = rt.bestRound;
            }
        }

        /*
        bestTimeOfAll = 0f;

        if (racers.Count > 0)
        {
            foreach (GameObject g in racers)
            {
                RoundTimer rt = g.GetComponent<RoundTimer>();

                if (rt != null)
                {
                    if (bestTimeOfAll == 0f || rt.bestRound < bestTimeOfAll)
                    {
                        bestTimeOfAll = rt.bestRound;
                        bestRacer = g;
                    }
                }
            }
        }

        return bestTimeOfAll;
        */
    }

    /*
    public void UpdatePlacements()
    {
        RefreshRacers();

        List<GameObject> lap1 = new List<GameObject>();
        List<GameObject> lap2 = new List<GameObject>();
        List<GameObject> lap3 = new List<GameObject>();

        //SORT WITH LAP COUNT
        if (racers.Count > 0)
        {
            foreach (GameObject rc in racers)
            {
                switch (rc.GetComponent<LapTracker>().lap)
                {
                    case 1:
                        lap1.Add(rc);

                        break;
                    case 2:
                        lap2.Add(rc);

                        break;
                    case 3:
                        lap3.Add(rc);

                        break;
                }
            }

            //SORT EACH LAP BY WP, IF MULTIPLE IN ONE SORT ALSO BY DISTANCE TO NEXT WP
            List<GameObject> lap1S = InsertionSort(lap1);
            List<GameObject> lap2S = InsertionSort(lap2);
            List<GameObject> lap3S = InsertionSort(lap3);

            racers.Clear();

            //COMBINE ALL SORTED LAPS BACK IN ONE LIST = RACERS
            for (int s = 0; s < lap3S.Count; s++)
            {
                racers.Add(lap3S[s]);
            }
            for (int s = 0; s < lap2S.Count; s++)
            {
                racers.Add(lap2S[s]);
            }
            for (int s = 0; s < lap1S.Count; s++)
            {
                racers.Add(lap1S[s]);
            }            
        }
    }
    */

    public void AddFinisher(GameObject g)
    {
        finishers.Add(g);
    }

    public int GetPosition(GameObject g)
    {
        for (int i = 0; i < racers.Count; i++)
        {
            if (racers[i] == g)
            {
                return i + 1;
            }
        }

        return -1;
    }

    public int GetPositionFinishers(GameObject g)
    {
        for (int i = 0; i < finishers.Count; i++)
        {
            if (finishers[i] == g)
            {
                return i + 1;
            }
        }

        return -1;
    }

    public int GetWP(GameObject g)
    {
        if (g.GetComponent<WaypointManager>() != null)
        {
            return g.GetComponent<WaypointManager>().currentWaypointIndex;
        }
        else
        {
            Debug.Log("GETWP CAN'T FIND A WAYPOINTMANAGER ON GAMEOBJECT!");
            return -1;
        }

    }

    /*
        private List<GameObject> InsertionSort(List<GameObject> arr)
        {
            //List<GameObject> oldArr = arr;

            //SORT BY WP NUMBER
            for (int i = 1; i < arr.Count; ++i)
            {
                GameObject oldVar = arr[i];
                int key = GetWP(arr[i]);
                int j = i - 1;

                while (j >= 0 && GetWP(arr[j]) < key)
                {
                    arr[j + 1] = arr[j];
                    j -= 1;
                }

                arr[j + 1] = oldVar;
            }

            List<int> _noD = new List<int>();
            for (int i = 0; i < arr.Count; i++)
            {
                _noD.Add(GetWP(arr[i]));
            }
            var noDupes = _noD.Distinct().ToList();

    //        
            if (oldArr != arr)
            {
    //        

            List<GameObject> arrNew = new List<GameObject>();

            //FOR EACH KART GET LIST WITH ALL KARTS WITH SAME WP
            for (int i = 0; i < noDupes.Count; i++)
            {
                List<GameObject> _arr = new List<GameObject>();
                _arr.Clear();

                foreach (GameObject go in arr)
                {
                    if (GetWP(go) == noDupes[i])
                    {
                        _arr.Add(go);
                    }
                }

                if (_arr.Count > 1)
                {
                    //SORT KART LIST WITH SAME WP BY DISTANCE TO NEXT WP
                    for (int t = 1; t < _arr.Count; ++t)
                    {
                        GameObject oldVal = _arr[t];
                        int key = Mathf.RoundToInt(Mathf.Abs(Vector3.Distance(_arr[t].transform.position, _arr[t].GetComponent<WaypointManager>().Waypoints[GetWP(_arr[t])].transform.position)));
                        int j = t - 1;

                        while (j >= 0 && Mathf.RoundToInt(Mathf.Abs(Vector3.Distance(_arr[j].transform.position, _arr[j].GetComponent<WaypointManager>().Waypoints[GetWP(_arr[j])].transform.position))) > key)
                        {
                            _arr[j + 1] = _arr[j];
                            j -= 1;
                        }
                        _arr[j + 1] = oldVal;
                    }

                    for (int v = 0; v < _arr.Count; v++)
                    {
                        arrNew.Add(_arr[v]);
                    }
                }
                else if (_arr.Count == 1)
                {
                    arrNew.Add(_arr[0]);
                }

            }

    //        
            for (int i = 0; i < arr.Count; i++)
                {
                    List<GameObject> _arr = new List<GameObject>();
                    _arr.Clear();
                    if (!arrNew.Contains(arr[i]))
                    {
                        _arr.Add(arr[i]);                    

                        for (int s = 0; s < arr.Count; s++)
                        {
                            if (GetWP(arr[s]) == GetWP(_arr[0]))
                            {
                                _arr.Add(arr[s]);
                            }
                        }

                        if (_arr.Count > 1)
                        {

                            //SORT KART LIST WITH SAME WP BY DISTANCE TO NEXT WP
                            for (int t = 1; t < _arr.Count; ++t)
                            {
                                GameObject oldVal = _arr[t];
                                int key = Mathf.RoundToInt(Mathf.Abs(Vector3.Distance(_arr[t].transform.position, _arr[t].GetComponent<WaypointManager>().Waypoints[GetWP(_arr[t])].transform.position)));
                                int j = t - 1;

                                while (j >= 0 && Mathf.RoundToInt(Mathf.Abs(Vector3.Distance(_arr[j].transform.position, _arr[j].GetComponent<WaypointManager>().Waypoints[GetWP(_arr[j])].transform.position))) > key)
                                {
                                    _arr[j + 1] = _arr[j];
                                    j -= 1;
                                }
                                _arr[j + 1] = oldVal;
                            }

                            for (int v = 0; v < _arr.Count; v++)
                            {
                                arrNew.Add(_arr[v]);
                            }
                        }
                        else
                        {
                            arrNew.Add(_arr[0]);
                        }
                    }
                }
            //}
    //        

                return arrNew;
        }
    */

    /*
        private void RefreshRacers()
        {
            racers.Clear();
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < gos.Length; i++)
            {
                racers.Add(gos[i]);
            }

            bestRacer = racers[Random.Range(0, racers.Count)];
        }
    */

    public void LoadEndscreenScene(bool a)
    {
        if (a == true)
        {
            GetBestTimeOverall();
            anim.SetTrigger("IngameO");
        }
        else
        {
            bestTimeOfAll = 0;
            anim.SetTrigger("SceneO");
        }
    }

    public string ConvertTimerInText(float a)
    {
        string _minutes;
        string _seconds;
        string _milseconds;      
        
        float minutes = Mathf.Floor(a / 60);
        float seconds = Mathf.RoundToInt(a % 60);
        float milseconds = Mathf.RoundToInt((a * 10) % 10) ;       
              
        _minutes = minutes.ToString();
        _seconds = seconds.ToString();
        _milseconds = milseconds.ToString();

        if (minutes < 10)
        {
            _minutes = "0" + minutes;
        }
        
        if (seconds < 10)
        {
            _seconds = "0" + Mathf.RoundToInt(seconds).ToString();
        }

        if (milseconds < 10)
        {
            Debug.Log("Cock");   
            _milseconds = "0" + Mathf.RoundToInt(milseconds).ToString();
          
        }
                
        return _minutes + ":" + _seconds + ":" + _milseconds;               
    }
    
}
