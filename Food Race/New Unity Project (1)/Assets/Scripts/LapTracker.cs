﻿using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTracker : MonoBehaviour
{
    public int lap;
    public float cpDist;
    public bool goalEnabled;
    public float goalDist;

    public Transform goalPoint;
    public List<Transform> checkpoints;
    public List<bool> checkpointsPassed;

    public bool finished;

    public KartAIAgent ka;

    void Start()
    {
        ResetAll();

        GameObject checkpointsGroup = GameObject.Find("Checkpoints");
        Transform[] cpG = checkpointsGroup.GetComponentsInChildren<Transform>();
        goalPoint = GameObject.Find("GoalPoint").transform;
        ka = GetComponent<KartAIAgent>();

        for (int i = 1; i < cpG.Length; i++)
        {
            checkpoints.Add(cpG[i]);
            checkpointsPassed.Add(false);
        }
    }

    void Update()
    {
        if (goalEnabled)
        {
            UpdateGoalCheck();
        }
        else
        {
            UpdateCheckpointCheck();
        }
    }

    public void PassCheckpoint(int s)
    {
        checkpointsPassed[s] = true;
        ka.AddReward(0.01f);
    }

    public bool CheckAllPassed()
    {
        if (checkpointsPassed[0] && checkpointsPassed[1] && checkpointsPassed[2] && checkpointsPassed[3])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetAllFalse()
    {
        for (int i = 0; i < checkpointsPassed.Count; i++)
        {
            checkpointsPassed[i] = false;
        }
    }

    public void UpdateCheckpointCheck()
    {
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

        for (int i = 0; i < checkpoints.Count; i++)
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
                ka.SetReward(1f);
                ka.EndEpisode();

            }

            SetAllFalse();
            goalEnabled = false;
            ka.AddReward(0.1f);
        }
    }

    public void ResetAll()
    {
        lap = 1;
        goalEnabled = false;
        finished = false;
        SetAllFalse();
    }


    private void OnDrawGizmos()     //visualize waypoint range in editor
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(gameObject.transform.position, cpDist);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, goalDist);
    }

}
