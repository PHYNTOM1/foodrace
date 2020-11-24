using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<WaypointBehaviour> wayPoints;

    private void Awake()
    {
        wayPoints = new List<WaypointBehaviour>(GetComponentsInChildren<WaypointBehaviour>());
    }
}
