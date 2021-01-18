using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "WPCollider")
        {
            if (c.gameObject.GetComponentInParent<LapTracker>() != null)
            {
                c.gameObject.GetComponentInParent<LapTracker>().PassGoal();
            }
        }
    }
}
