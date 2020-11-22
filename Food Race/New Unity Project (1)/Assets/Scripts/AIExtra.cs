﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

    public class AIExtra : MonoBehaviour
{
    public List<Transform> waypoints;

    public int currWP = 0;
    public float distWP = 0;
    public float wpDist = 10;

    private Rigidbody kartRB;

    private bool driftInput = false;
    private bool driftInputDown = false;
    private float horAxis = 0f;
    private float horAxisRaw = 0f;
    private float verAxis = 0f;
    private float verAxisRaw = 0f;

    private Vector3 targetDir;


    void Start()
    {
        gameObject.GetComponent<CartController>().isPlayer = false;
        kartRB = gameObject.GetComponent<CartController>().theRB;

        GameObject waypointsGroup = GameObject.Find("RoundWaypoints");
        Transform[] wpG = waypointsGroup.GetComponentsInChildren<Transform>();

        for (int i = 1; i < wpG.Length; i++)
        {
            waypoints.Add(wpG[i]);
        }
    }

    private void UpdateWaypointsLocal()     //if cart is in wpDist range to current target waypoint set next waypoint as target
    {
        distWP = Vector3.Distance(transform.position, waypoints[currWP].position);

        if (distWP >= wpDist * 2)
        {
            verAxis = 1;
            verAxisRaw = 1;
        }
        else if (distWP <= wpDist)
        {
            currWP++;
            verAxisRaw = 0f;
        }
    }

    void Update()
    {
        UpdateWaypointsLocal();


        //COMPARE Y AXIS ROT BETWEEN TARGETPOS & CURR .FORWARD
        targetDir = waypoints[currWP].position - kartRB.position;
        //targetDir.y = kartRB.position.y;        

        float angle = Vector3.Angle(targetDir, kartRB.transform.forward);

        Vector3 kartForPos = kartRB.transform.position - (kartRB.transform.forward.normalized * 5);     //remove * 5
        kartForPos.y = kartRB.transform.position.y;

        Vector3 kartToWP = kartRB.transform.position + targetDir.normalized * 5;    //remove * 5
        kartToWP.y = kartRB.transform.position.y;

        Debug.DrawLine(kartRB.transform.position, kartForPos, Color.magenta);   //JUST DEBUGGING
        Debug.DrawLine(kartRB.transform.position, kartToWP, Color.yellow);

        Vector3 xSHIT = kartToWP - kartForPos;

        Debug.DrawLine(kartForPos, kartForPos + xSHIT);

        if (xSHIT.x > 0)
        {
            Debug.Log("++++++++++++++++++++++++");
            //angle *= -1;
        }
        else if (xSHIT.x < 0)
        {
            Debug.Log("------------------------");
        }

        Debug.Log(gameObject.name + " :angle: " + angle);

        if (angle > 110 && angle < 180)
        {
            horAxis = 1;
            horAxisRaw = 1;
        }
        else if (angle < -110 && angle > -180)
        {
            horAxis = -1;
            horAxisRaw = -1;
        }
        else if (angle > 60 && angle < 180)
        {
            horAxis = 0.7f;
            horAxisRaw = 1;
        }
        else if (angle < -60 && angle > -180)
        {
            horAxis = -0.7f;
            horAxisRaw = -1;
        }
        else if (angle <= 5 && angle > 0)
        {
            horAxis = 0;
            horAxisRaw = 0;
        }
        else if (angle >= -5 && angle < 0)
        {
            horAxis = 0;
            horAxisRaw = 0;
        }

        Debug.Log(gameObject.name + horAxis);

    }

    public bool DriftOutput()
    {
        return driftInput;
    }

    public bool DriftDownOutput()
    {
        return driftInputDown;
    }
    
    public float HorAxis()
    {       
        return horAxis;
    }
    
    public float HorAxisRaw()
    {
        return horAxisRaw;
    }

    public float VerAxis()
    {
        return verAxis;
    }

    public float VerAxisRaw()
    {
        return verAxisRaw;
    }


    private void OnDrawGizmos()     //visualize waypoint range in editor
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, wpDist);
        Gizmos.DrawLine(kartRB.position, waypoints[currWP].position);
        Gizmos.DrawLine(kartRB.position, kartRB.position + targetDir);
    }
}
