using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBehaviour : MonoBehaviour
{
    public int cpNum;

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "WPCollider")
        {
            if (c.gameObject.GetComponentInParent<LapTracker>() != null)
            {
                c.gameObject.GetComponentInParent<LapTracker>().PassCheckpoint(cpNum);
            }
            else
            {
            }
        }
    }
}
