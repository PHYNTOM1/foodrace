using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider c)
    {
        if (c.name == "CartColl")
        {
            //SceneManager.LoadScene(1);

            /*
            c.gameObject.GetComponent<CartController>().lap++;
            Debug.Log(c.gameObject.name + "completed " + c.gameObject.GetComponent<CartController>().lap + ". lap!");

            if (c.gameObject.GetComponent<CartController>().lap == 2)
            {
                Debug.Log(c.gameObject.name + " has FINISHED!");
                Destroy(c.gameObject, 1f);
            }
            */
        }
    }

}


/* 
    
    public List<Transform> waypoints;

    public int currWP = 0;
    public float distWP = 0;
    public float wpDist = 5;



    GameObject waypointsGroup = GameObject.Find("RoundWaypoints");
    Transform[] wpG = waypointsGroup.GetComponentsInChildren<Transform>();

    for (int i = 1; i < wpG.Length; i++)
    {
         waypoints.Add(wpG[i]);
    }

    
    
    private void UpdateWaypointsLocal()     //if cart is in wpDist range to current target waypoint set next waypoint as target
    {
        distWP = Vector3.Distance(transform.position, waypoints[currWP].position);

        if (distWP <= wpDist)
        {
            currWP++;
        }
    }


    private void OnDrawGizmos()     //visualize waypoint range in editor
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, wpDist);
    }
  
 */
