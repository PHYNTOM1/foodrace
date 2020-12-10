using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public float maxTimeToReachNextWaypoint = 20f;
    public float timeLeft = 20f;

    public KartAIAgent kartAIAgent;
    public WaypointBehaviour nextWayPointToReach;

    private int currentWaypointIndex;
    private List<WaypointBehaviour> Waypoints;
    private WaypointBehaviour lastWaypoint;
    private LapTracker lt;

    public event Action<WaypointBehaviour> reachedWaypoint;

    void Start()
    {
        /*
        */
        kartAIAgent = GetComponent<KartAIAgent>();
        Waypoints = FindObjectOfType<Waypoints>().wayPoints;
        lt = GetComponent<LapTracker>();
        ResetWPs();
    }

    void Update()
    {
        //ENABLE JUST FOR TRAINING
        
        /*
         */
        timeLeft -= Time.deltaTime;

        if (lt.finished == true)
        {
            ResetWPs();
        }

        if (timeLeft < 0f)
        {
            if (lt.finished == false)
            {
                kartAIAgent.AddReward(-1f);
            }

            kartAIAgent.EndEpisode();
        }
    }

    public void ReachedWaypoint(WaypointBehaviour waypointBehaviour)
    {
        if (nextWayPointToReach != waypointBehaviour) return;

        lastWaypoint = Waypoints[currentWaypointIndex];
        reachedWaypoint?.Invoke(waypointBehaviour);
        currentWaypointIndex++;

        if (currentWaypointIndex >= Waypoints.Count)
        {
            kartAIAgent.AddReward(0.5f);
            ResetWPs();
        }
        else
        {
            kartAIAgent.AddReward((0.5f) / Waypoints.Count);
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        if (Waypoints.Count > 0)
        {
            timeLeft = maxTimeToReachNextWaypoint;
            nextWayPointToReach = Waypoints[currentWaypointIndex];
        }
    }

    public void ResetWPs()
    {
        currentWaypointIndex = 0;
        timeLeft = maxTimeToReachNextWaypoint;

        SetNextWaypoint();
    }
}
