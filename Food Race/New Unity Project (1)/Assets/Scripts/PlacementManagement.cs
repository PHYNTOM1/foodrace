using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlacementManagement : MonoBehaviour
{
    public List<GameObject> racers;
    public List<GameObject> finishers;
    public float bestTimeOfAll;
    public GameObject bestRacer;

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
            DontDestroyOnLoad(_instance);
        }
    }

    void Start()
    {      
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        Debug.Log("NEW SCENE LOADED: " + aScene.name);
        if (aScene.name == "MainMenu")
        {
            if (finishers.Count == racers.Count)
            {
                RankingEndscreen re = FindObjectOfType<RankingEndscreen>();
                re.gameObject.SetActive(true);
                GameObject.Find("MainMenu").GetComponent<MainMenu>().LoadRankings();
            }
        }
        else if (aScene.name == "Ingame2")
        {
            if (finishers.Count > 0)
            {
                for (int i = 0; i < finishers.Count; i++)
                {
                    Destroy(finishers[i].gameObject);
                }
            }
            bestTimeOfAll = 0f;
            finishers.Clear();
            RefreshRacers();            
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Ingame2")
        {
            UpdatePlacements();

            if (finishers.Count == racers.Count)
            {
                LoadEndscreenScene();
            }
        }
    }    

    public float GetBestTimeOverall()
    {
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
    }

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

        /*
        if (oldArr != arr)
        {
        */

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

        /*
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
        */

            return arrNew;
    }

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

    public void LoadEndscreenScene()
    {
        for (int i = 0; i < finishers.Count; i++)
        {
            finishers[i].GetComponent<CartController>().enabled = false;
            finishers[i].GetComponent<KartAIAgent>().enabled = false;
            finishers[i].GetComponent<WaypointManager>().enabled = false;
            finishers[i].GetComponent<LapTracker>().enabled = false;

            DontDestroyOnLoad(finishers[i].gameObject);
        }
        SceneManager.LoadScene(0);
    }
}
