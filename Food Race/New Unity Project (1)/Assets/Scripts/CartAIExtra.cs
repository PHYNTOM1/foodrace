using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartAIExtra : MonoBehaviour
{
    public Transform[] waypoints;

    private int currWP = 0;
    private float distWP = 0;


    void Start()
    {
        gameObject.GetComponent<CartController>().isPlayer = false;

        GameObject waypointsGroup = GameObject.Find("RoundWaypoints");
        waypoints = waypointsGroup.GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        gameObject.transform.LookAt(waypoints[currWP + 1]);

        distWP = Vector3.Distance(transform.position, waypoints[currWP + 1].position);

        if (distWP <= 20)
        {
            currWP++;
        }

        CheckForRaceFinish();

    }

    public void AIMovementInput( out  float vert, out float hor)
    {
        vert = 0;
        hor = 1;

    }

    public void CheckForRaceFinish()
    {
        if (gameObject.GetComponent<CartController>().lap == 2)
        {
            Debug.Log(gameObject.name + " has FINISHED!");
            Destroy(gameObject, 1f);
        }
    }

}
