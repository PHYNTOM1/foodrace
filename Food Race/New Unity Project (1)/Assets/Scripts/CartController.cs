using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class CartController : MonoBehaviour
{

    public WheelCollider[] wheelColls;      //array of all wheelcolliders
    public GameObject[] wheelMeshs;     //array of all wheel meshs (visible wheels)
    public GameObject cartMesh;     //cart mesh object ("visible rigidbody" if you want to call it that way)

    [Space]
    public Transform massCenter;        //Center of mass on the cart rigidbody
    public Rigidbody cartRB;        //rigidbody ("collider") of the cart

    [Space]
    public CartStats cartStats;     //easily in inspector swappable cart stats
    public float maxMotorTorque;        //simple variable for higher values through multiplication (on motor)
    public float maxSteering;       //simple variable for higher values through multiplication (on steering)

    private float motor = 0;    //"acceleration", the amount of power applied to each wheelcollider (here all 4 wheels)
    private float steering = 0;     //"handling", the degree of turns applied to each wheelcollider (here only 2 front wheels steering)
    private bool isDrifting = false;        //bool check for drifting; is true while holding SHIFT  => in FixedUpdate()

    public bool isPlayer = true;        //bool difference between player or AI controlled
    public int lap = 1;     //the amount of laps this cart is currently in (not working yet)

    void Start()
    {
        cartRB = gameObject.GetComponent<Rigidbody>();  //get cart rigidbody and set CoM
        cartRB.centerOfMass = massCenter.localPosition;
    }

    void Update()
    {
        if (isPlayer)   //player controls carts with input, AI controls carts with internal logic from "CartAIExtra" script
        {
            float VertInput = Input.GetAxis("Vertical");
            float HorInput = Input.GetAxis("Horizontal");

            SetWheelVars(VertInput, HorInput);
        }
        else
        {
            SetWheelVars(gameObject.GetComponent<CartAIExtra>().AIMovementInput().y, gameObject.GetComponent<CartAIExtra>().AIMovementInput().x);
        }



        if (Input.GetKeyDown(KeyCode.LeftShift))    //simple NOT WORKING drift mechanic
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }
    }

    void FixedUpdate()
    {
        if (isDrifting)     //decrease carts turn radius and apply a sideways force, dependend on steering direction
        {
            //ACHIS SUPERDRIFTING HIER!!!!11!1
            
        }

        wheelColls[0].steerAngle = steering;    //apply steering variable to wheelcolliders; only front wheel steering
        wheelColls[1].steerAngle = steering;

        foreach (WheelCollider wc in wheelColls)    //apply motor variable to wheelcolliders; four wheel drive, motor powers all wheels
        {
            wc.motorTorque = motor;

        }

        if (cartRB.velocity.sqrMagnitude > Mathf.Pow(cartStats.topSpeed, 2)) //check if current speed greater than defined topSpeed
        {
            cartRB.velocity *= 0.99f;   //decrease cart speed (lower (i.e. 0.6) is less smooth)
        }

    }

    void LateUpdate()   //rotate wheel meshes according to wheelcollider rotations after every other logic has run -> Late
    {
        CollToMeshRotation();
    }



    public void CollToMeshRotation()
    {
        Quaternion rot;
        Vector3 pos;

        for (int i = 0; i < wheelColls.Length; i++)     //update all wheels, get pos / rot from colliders and apply to meshs
        {
            wheelColls[i].GetWorldPose(out pos, out rot);
            wheelMeshs[i].transform.position = pos;
            wheelMeshs[i].transform.rotation = rot;
            wheelMeshs[i].transform.eulerAngles = new Vector3(0, rot.eulerAngles.y, 90);
        }
    }


    public void SetWheelVars(float ver, float hor)    //convert input to wheelcollider variables
    {
        motor = maxMotorTorque* cartStats.acceleration * ver;  
        steering = maxSteering* cartStats.handling * hor;
    }
}
