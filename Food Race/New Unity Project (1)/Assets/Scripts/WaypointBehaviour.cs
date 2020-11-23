using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LapTracker>() != null)
        {
            //other.GetComponent<LapTracker>().reachedWaypoint();
        }
    }
}
