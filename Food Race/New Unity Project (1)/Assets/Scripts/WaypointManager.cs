using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public float maxTimeToReachNextWaypoint = 30f;
    public float timeLeft = 30f;

    public KartAIAgent kartAIAgent;
    public WaypointBehaviour nextWayPointToReach;

    private int currentWaypointIndex;
    private List<WaypointBehaviour> Waypoints;
    private WaypointBehaviour lastWaypoint;

    public event Action<WaypointBehaviour> reachedWaypoint;

    void Start()
    {
        kartAIAgent = GetComponent<KartAIAgent>();
        Waypoints = FindObjectOfType<Waypoints>().wayPoints;
        ResetWPs();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0f)
        {
            kartAIAgent.AddReward(-1f);
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
            kartAIAgent.EndEpisode();
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
