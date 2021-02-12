using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTracker : MonoBehaviour
{
    public int lap;
    //    public float cpDist;
    //    public bool goalEnabled;
    //    public float goalDist;

    //    public Transform goalPoint;
    public List<Transform> checkpoints;
    public List<bool> checkpointsPassed;

    public bool finished = false;

    //public KartAIAgent ka;
    public RoundTimer rt;
    public EnemyRespawning er1;
    public EnemyRespawning er2;
    public EnemyRespawning er3;

    public SoundManagement sm;

    void Start()
    {
        //goalPoint = GameObject.Find("GoalPoint").transform;
        //ka = GetComponent<KartAIAgent>();
        sm = PlacementManagement.Instance.GetComponent<SoundManagement>();
        rt = GetComponent<RoundTimer>();
        er1 = GameObject.Find("Map01").GetComponentInChildren<EnemyRespawning>();
        er2 = GameObject.Find("Map02").GetComponentInChildren<EnemyRespawning>();
        er3 = GameObject.Find("Map03").GetComponentInChildren<EnemyRespawning>();

        checkpoints.Clear();
        checkpointsPassed.Clear();
        GameObject checkpointsGroup = GameObject.Find("Checkpoints");
        Transform[] cpG = checkpointsGroup.GetComponentsInChildren<Transform>();

        for (int i = 1; i < cpG.Length; i++)
        {
            checkpoints.Add(cpG[i]);
            checkpointsPassed.Add(false);
        }

        ResetAll();
    }

    void Update()
    {
        if (sm == null)
        {
            sm = PlacementManagement.Instance.GetComponent<SoundManagement>();
        }
        if (rt == null)
        {
            rt = GetComponent<RoundTimer>();
        }
        if (er1 == null)
        {
            er1 = GameObject.Find("Map01").GetComponentInChildren<EnemyRespawning>();
        }
        if (er2 == null)
        {
            er2 = GameObject.Find("Map02").GetComponentInChildren<EnemyRespawning>();
        }
        if (er3 == null)
        {
            er3 = GameObject.Find("Map03").GetComponentInChildren<EnemyRespawning>();
        }
        if (checkpoints.Count <= 0)
        {
            checkpoints.Clear();
            checkpointsPassed.Clear();
            GameObject checkpointsGroup = GameObject.Find("Checkpoints");
            Transform[] cpG = checkpointsGroup.GetComponentsInChildren<Transform>();

            for (int i = 1; i < cpG.Length; i++)
            {
                checkpoints.Add(cpG[i]);
                checkpointsPassed.Add(false);
            }
        }
    }

    public void PassCheckpoint(int s)
    {
        sm.PlayOneShot("check6");
        checkpointsPassed[s] = true;
        rt.CompletedCP(s);
        //ka.AddReward(0.01f);
    }

    public bool CheckAllPassed()
    {
        bool result = false;
        foreach (bool b in checkpointsPassed)
        {
            result = b;

            if (result == false)
            {
                return result;
            }
        }

        return result;
    }

    public void SetAllFalse()
    {
        for (int i = 0; i < checkpointsPassed.Count; i++)
        {
            checkpointsPassed[i] = false;
        }
    }

    //    public void UpdateCheckpointCheck()
    //    {
    /*
    PlacementManagement pm = FindObjectOfType<PlacementManagement>();
    for (int i = 0; i < pm.racers.Count; i++)
    {
        if (pm.racers[i] != this.gameObject)
        {
            float distKart = Vector3.Distance(gameObject.transform.position, pm.racers[i].transform.position);

            if (distKart <= cpDist)
            {
                Debug.Log(pm.racers[i].name + " entered " + gameObject.name + "'s zone!: UPDATE PLACEMENTS");

                pm.UpdatePlacements();
            }
        }
    }
    */

    /*        for (int i = 0; i < checkpoints.Count; i++)
            {
                float distWP = Vector3.Distance(gameObject.transform.position, checkpoints[i].position);

                if (distWP <= cpDist)
                {
                    Debug.Log(gameObject.name + "checked " + checkpoints[i].name);

                    PassCheckpoint(i);
                }
            }

            if (CheckAllPassed())
            {
                goalEnabled = true;
            }
        }
    */

    /*
    public void UpdateGoalCheck()
    {
        float distWP = Vector3.Distance(gameObject.transform.position, goalPoint.position);

        if (distWP <= goalDist)
        {
            Debug.Log(gameObject.name + "PASSED GOAL!");
            lap++;

            if (lap == 4)
            {
                //DO SOME FINISHING SHIT HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                Debug.Log(gameObject.name + " HAS FINISHED THE RACE!");
                finished = true;
                PlacementManagement.Instance.AddFinisher(this.gameObject);
                /*
                ka.SetReward(1f);
                ka.EndEpisode();
                */
    /*            }

                SetAllFalse();
                goalEnabled = false;
                //ka.AddReward(0.1f);
            }
        }
        */

    public void PassGoal()
    {
        if (CheckAllPassed())
        {
            Debug.Log(gameObject.name + "PASSED GOAL!");
            lap++;
            rt.CompletedRound(lap);
            sm.PlayOneShot("check");

            switch (PlacementManagement.Instance.selectedMap)
            {
                case 0:

                    er1.RespawnEnemies();
                    break;
                case 1:

                    er2.RespawnEnemies();
                    break;
                case 2:

                    er3.RespawnEnemies();
                    break;
            }

            //FINISH GAME!!!
            if (lap == 4)
            {
                Debug.Log(gameObject.name + " HAS FINISHED THE RACE!");
                finished = true;
                PlacementManagement.Instance.AddFinisher(this.gameObject);
                PlacementManagement.Instance.finished = finished;
                PlacementManagement.Instance.LoadEndscreenScene(true);
            }

            SetAllFalse();
        }
    }

    public void ResetAll()
    {
        lap = 1;
        //goalEnabled = false;
        finished = false;
        PlacementManagement.Instance.finished = finished;

        switch (PlacementManagement.Instance.selectedMap)
        {
            case 0:

                er1.RespawnEnemies();
                break;
            case 1:

                er2.RespawnEnemies();
                break;
            case 2:

                er3.RespawnEnemies();
                break;
        }

        SetAllFalse();
    }

    /*
    private void OnDrawGizmos()     //visualize waypoint range in editor
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(gameObject.transform.position, cpDist);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, goalDist);
    }
    */

}
