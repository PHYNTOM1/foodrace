using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WPCollider")
        {        
            if(other.gameObject.GetComponentInParent<WaypointManager>() != null)
            {
                other.gameObject.GetComponentInParent<WaypointManager>().ReachedWaypoint(this);
            }
        }
    }
}
