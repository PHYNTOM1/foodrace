using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBehaviour : MonoBehaviour
{
    public int cpNum = 0;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "WPCollider")
        {
            if (c.gameObject.GetComponentInParent<LapTracker>() != null)
            {
                c.gameObject.GetComponentInParent<LapTracker>().PassCheckpoint(cpNum);
            }
        }
    }
}
