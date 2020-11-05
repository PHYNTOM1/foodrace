using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class CartController : MonoBehaviour
{

    public WheelCollider[] wheelColls;
    public GameObject[] wheelMeshs;
    public GameObject cartMesh;

    [Space]
    public Transform massCenter;
    public Rigidbody cartRB;

    [Space]
    public CartStats cartStats;
    public float maxMotorTorque;
    public float maxSteering;

    private float motor = 0;
    private float steering = 0;
    private bool isDrifting = false;

    public bool isPlayer = true;
    public int lap = 1;

    void Start()
    {
        cartRB = gameObject.GetComponent<Rigidbody>();  //get cart rigidbody and set CoM
        cartRB.centerOfMass = massCenter.localPosition;
    }

    void Update()
    {
        if (isPlayer)
        {
            float VertInput = Input.GetAxis("Vertical");
            float HorInput = Input.GetAxis("Horizontal");

            SetWheelVars(VertInput, HorInput);
        }
        else
        {
            //SetWheelVars();
        }



        if (Input.GetKeyDown(KeyCode.LeftShift))
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
        if (isDrifting)
        {
            steering *= 2;
            if (steering > 0)
            {
                cartRB.AddForce(new Vector3(1, 0, 0), ForceMode.Force);
            }
            else if (steering < 0)
            {
                cartRB.AddForce(new Vector3(-1, 0, 0), ForceMode.Force);
            }
        }

        wheelColls[0].steerAngle = steering;    //only front wheel steering
        wheelColls[1].steerAngle = steering;

        foreach (WheelCollider wc in wheelColls)    //four wheel drive, motor powers all wheels
        {
            wc.motorTorque = motor;

        }

        if (cartRB.velocity.sqrMagnitude > Mathf.Pow(cartStats.topSpeed, 2)) //check if current speed greater than defined topSpeed
        {
            cartRB.velocity *= 0.99f;   //decrease cart speed (lower (i.e. 0.6) is less smooth)
        }

    }

    void LateUpdate()
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
