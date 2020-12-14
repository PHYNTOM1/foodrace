using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManagement : MonoBehaviour
{
    public List<GameObject> racers;
    public float bestTimeOfAll;

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
        bestTimeOfAll = 0f;
        racers.Clear();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        
        for (int i = 0; i < gos.Length; i++)
        {
            racers.Add(gos[i]);
        }
    }

    void Update()
    {
        UpdatePlacements();
    }

    public float GetBestTimeOverall()
    {
        foreach (GameObject g in racers)
        {
            if (g.GetComponent<RoundTimer>() != null)
            {
                RoundTimer rt = g.GetComponent<RoundTimer>();
                
                if (bestTimeOfAll == 0f || rt.bestRound < bestTimeOfAll)
                {
                    bestTimeOfAll = rt.bestRound;
                }
            }
        }

        return bestTimeOfAll;
    }

    public void SortByPosition()
    {
        foreach (GameObject g in racers)
        {
            if (true)
            {
            
            }
        }
    }

    public void UpdatePlacements()
    {
        //SORT WITH LAP COUNT
        if (racers.Count > 0)
        {
            racers.Sort(delegate (GameObject a, GameObject b)
            {
                return (a.GetComponent<LapTracker>().lap).CompareTo(b.GetComponent<LapTracker>().lap);
            });

            racers.Reverse();
        }
    }
}
