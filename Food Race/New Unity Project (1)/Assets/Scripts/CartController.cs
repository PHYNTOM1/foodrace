using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CartController : MonoBehaviour
{

    public WheelCollider[] wheelColls;
    public GameObject[] wheelMeshs;
    
    [Space]
    public Transform massCenter;
    public Rigidbody cartRB;

    [Space]
    public CartStats cartStats;
    public float maxMotorTorque;
    public float maxSteering;

    private float motor = 0;
    private float steering = 0;

    void Start()
    {
        cartRB = gameObject.GetComponent<Rigidbody>();  //get cart rigidbody and set CoM
        cartRB.centerOfMass = massCenter.localPosition;
    }

    void Update()
    {
        motor = maxMotorTorque * cartStats.acceleration * Input.GetAxis("Vertical");  //convert input to wheelcollider variables
        steering = maxSteering * cartStats.handling * Input.GetAxis("Horizontal");    
    }

    void FixedUpdate()
    {
        wheelColls[0].steerAngle = steering;    //only front wheel steering
        wheelColls[1].steerAngle = steering;

        foreach (WheelCollider wc in wheelColls)    //four wheel drive, motor powers all wheels
        {
            wc.motorTorque = motor;

        }

        /*
        float speed = Vector3.Magnitude(cartRB.velocity);  //get current speed

        if (speed > cartStats.topSpeed)
        {
            Debug.Log("OVER SPEED");
            float brakeSpeed = speed - cartStats.topSpeed;  //calculate the speed decrease

            Vector3 normalisedVelocity = cartRB.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  //make the brake Vector3 value
            Debug.Log(brakeVelocity);

            cartRB.AddForce(-brakeVelocity);  //apply "negative" brake force
        }
        */
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
            wheelColls[i].GetWorldPose( out pos, out rot);
            wheelMeshs[i].transform.position = pos;
            wheelMeshs[i].transform.rotation = rot;
            wheelMeshs[i].transform.eulerAngles = new Vector3(0, rot.eulerAngles.y, 90);
        }
    }
}
