using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartController : MonoBehaviour
{

    public KartStats kartStats;
    public Transform kartModel;
    public Transform kartNormal;
    public LayerMask layerMask;

    public float acceleration = 3f;
    public float steering = 30f;
    public float gravity = 10f;

    private Rigidbody kartRB;
    private float speed, currentSpeed;
    private float rotate, currentRotate;

    //drifting
    public bool drifting;
    private float driftPower;
    private int driftDirection;
    private int driftMode = 0;
    private bool first, second, third;

    void Start()
    {
        kartRB = gameObject.GetComponentInChildren<Rigidbody>();    
    }

    void Update()
    {
        //rb pos to mesh
        transform.position = kartRB.transform.position - new Vector3(0, 0.5f, 0);

        //forward movement
        if (Input.GetAxisRaw("Vertical") > 0)
            speed = acceleration;

        //steering
        if (Input.GetAxis("Horizontal") != 0)
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs((Input.GetAxis("Horizontal")));
            Steer(dir, amount);
        }

        //drifting
        if (Input.GetButtonDown("Jump") && !drifting && Input.GetAxis("Horizontal") != 0)
        {
            drifting = true;
            driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
        }

        if (drifting)
        {
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 0, 2) : ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 2, 0);
            float powerControl = (driftDirection == 1) ? ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, .2f, 1) : ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 1, .2f);
            Steer(driftDirection, control);
            driftPower += powerControl;
        }

        if (Input.GetButtonUp("Jump") && drifting)
        {
            Boost();
        }

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f); 
        speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); 
        rotate = 0f;
    }

    void FixedUpdate()
    {
        //forward Acceleration
        if (drifting)
            kartRB.AddForce(-kartModel.transform.right * currentSpeed, ForceMode.Acceleration);
        else
            kartRB.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);

        //gravity
        //kartRB.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f, layerMask);

        //Normal Rotation
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);
    }


    public void Boost()
    {
        drifting = false;

        driftPower = 0;
        driftMode = 0;
        first = false; second = false; third = false;
    }

    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }

    private void Speed(float x)
    {
        currentSpeed = x;
    }


}
