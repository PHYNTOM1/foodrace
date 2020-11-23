using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

    public class AIExtra : MonoBehaviour
{
    public List<Transform> waypoints;

    public int currWP = 0;
    public float distWP = 0;
    private float oldDist = 0;
    public float wpDist = 10;

    private Rigidbody kartRB;
    private GameObject kart;

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
        kart = this.gameObject;

        GameObject waypointsGroup = GameObject.Find("RoundWaypoints");
        Transform[] wpG = waypointsGroup.GetComponentsInChildren<Transform>();

        for (int i = 1; i < wpG.Length; i++)
        {
            waypoints.Add(wpG[i]);
        }
    }

    private void UpdateWaypointsLocal()     //if cart is in wpDist range to current target waypoint set next waypoint as target
    {
        oldDist = distWP;
        distWP = Vector3.Distance(transform.position, waypoints[currWP].position);
    }

    private void CalculateMovement()
    {
        //COMPARE ROT BETWEEN TARGETPOS & CURR .FORWARD
        targetDir = waypoints[currWP].position - kart.transform.position;   //span a vector from kartposition to current waypoint position

        Vector3 kartForPos = kart.transform.position + (kart.transform.forward.normalized * 5);     //span a vector going forward 5 units from kart position (in local space)
        Vector3 kartToWP = kart.transform.position + targetDir.normalized * 5;    //span a vector going 5 units in targetDir from kart position

        kartForPos.y = kart.transform.position.y;   //set every vector to the same height as the kart
        kartToWP.y = kart.transform.position.y;


        Vector3 xSHIT = kartToWP - kartForPos;      //get the difference between kart forward and kart-waypoint vectors
        float angle = Vector3.Angle(targetDir, kart.transform.forward);     //get difference between targetDir and kart forward in degrees
        //float angle = Vector3.Angle(kartToWP, kartForPos);
        float dot = Vector3.Dot(kartToWP.normalized, kartForPos.normalized);
        float SHITFUCKINGVECTORLENGTH = Mathf.Abs(Vector3.SqrMagnitude(kartForPos));
        float SHITFUCKINGVECTORLENGTH2 = Mathf.Abs(Vector3.SqrMagnitude(xSHIT));

        Debug.DrawLine(kart.transform.position, kartForPos, Color.magenta);   //JUST DEBUGGING, showing the vectors
        Debug.DrawLine(kart.transform.position, kartToWP, Color.yellow);
        Debug.DrawLine(kartForPos, kartForPos + xSHIT);


        if (dot <= 0.1f || SHITFUCKINGVECTORLENGTH2 > SHITFUCKINGVECTORLENGTH)    //means the waypoint is directly to the karts side or behind = no motor force
        {
            verAxis = 0f;
            verAxisRaw = 0f;
        }
        else if (distWP >= wpDist * 1)   //multiply wpDist to extend range at which karts start to slow down
        {
            verAxis = 1;
            verAxisRaw = 1;
        }
        else if (distWP <= wpDist)
        {
            currWP++;
            verAxis = 0f;
            verAxisRaw = 0f;
        }

        if (xSHIT.x < 0)    //if x position < 0 determines whether its left or right (* -1 sets angle to other side, because difference calculation returns only positive)
        {
            angle *= -1;
        }

        if (gameObject.name == "AIKart (1)")
        {
            Debug.Log(gameObject.name + "<color=green> :angle: </color>" + angle);
            Debug.Log(gameObject.name + "<color=red> :dot: </color>" + dot);
        }

        if (angle > 50 && angle < 180)
        {
            horAxis = 1;
            horAxisRaw = 1;
            driftInput = true;
            driftInputDown = true;
        }
        else if (angle < -50 && angle > -180)
        {
            horAxis = -1;
            horAxisRaw = -1;
            driftInput = true;
            driftInputDown = true;
        }
        else if (angle > 30 && angle < 180)
        {
            horAxis = 0.7f;
            horAxisRaw = 1;
            driftInput = false;
            driftInputDown = false;
        }
        else if (angle < -30 && angle > -180)
        {
            horAxis = -0.7f;
            horAxisRaw = -1;
            driftInput = false;
            driftInputDown = false;
        }
        else if (angle <= 5 && angle > 0)
        {
            horAxis = 0;
            horAxisRaw = 0;
            driftInput = false;
            driftInputDown = false;
        }
        else if (angle >= -5 && angle < 0)
        {
            horAxis = 0;
            horAxisRaw = 0;
            driftInput = false;
            driftInputDown = false;
        }
    }

    void Update()
    {
        UpdateWaypointsLocal();

        CalculateMovement();
    }


    #region Getters
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
    #endregion


    private void OnDrawGizmos()     //visualize waypoint range in editor
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, wpDist);
        Gizmos.DrawLine(kartRB.position, waypoints[currWP].position);
        Gizmos.DrawLine(kartRB.position, kartRB.position + targetDir);
    }
}
