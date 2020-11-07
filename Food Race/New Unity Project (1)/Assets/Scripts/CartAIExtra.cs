using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class CartAIExtra : MonoBehaviour
{
    public List<Transform> waypoints;

    public int currWP = 0;
    public float distWP = 0;
    private Vector3 rot = Vector3.zero;
    public float wpDist = 5;

    public Color randAIColor;
    public MeshRenderer CartMesh;


    void Start()
    {
        gameObject.GetComponent<CartController>().isPlayer = false;

        randAIColor = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        CartMesh = gameObject.GetComponentInChildren<MeshRenderer>();
        CartMesh.material.color = randAIColor;

        GameObject waypointsGroup = GameObject.Find("RoundWaypoints");
        Transform[] wpG = waypointsGroup.GetComponentsInChildren<Transform>();

        for (int i = 1; i < wpG.Length; i++)
        {
            waypoints.Add(wpG[i]);
        }

    }

    void Update()
    {
        //gameObject.transform.LookAt(waypoints[currWP + 1]);

        UpdateWaypointsLocal();

    }

    public Vector2 AIMovementInput()    //generate AI "input" to move cart in "CartController" script; x = ver, y = hor input (-1 <-> 1)
    {
        return new Vector2(0, 1);

    }

    private void UpdateWaypointsLocal()     //if cart is in wpDist range to current target waypoint set next waypoint as target
    {
        distWP = Vector3.Distance(transform.position, waypoints[currWP].position);
        Vector3 rot = transform.InverseTransformDirection(new Vector3(waypoints[currWP].transform.position.x, transform.position.y, waypoints[currWP].transform.position.z));

        if (distWP <= wpDist)
        {
            currWP++;
        }
    }


    private void OnDrawGizmos()     //visualize waypoint range in editor
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, wpDist);
    }
}
