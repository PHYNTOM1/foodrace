using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalController : MonoBehaviour
{

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.GetComponent<LapTracker>() != null)
        {
            Debug.Log(c.gameObject.name + " passed goal!");
            LapTracker lt = c.gameObject.GetComponent<LapTracker>();

            if (lt.CheckAllPassed())
            {
                Debug.Log(c.gameObject.name + " completed " + lt.lap + ". lap!");
                lt.lap++;
                lt.SetAllFalse();
            }

            /*
            if (lt.lap == 3)
            {
                Debug.Log(c.gameObject.name + " has FINISHED!");
                c.gameObject.SetActive(false);
            }
            */
            
        }
    }

}
