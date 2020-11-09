using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public Rigidbody theRB;

    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180, gravityForce = 10f;

    public float speedInput, turnInput;

    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLenght = .5f;
    public Transform groundRayPoint;

    public Transform leftfrontwheel, rightfrontwheel;
    public float maxWheelTurn = 25f;

    public ParticleSystem[] exhaust;
    public float maxEmission = 25f;
    private float emissionRate;

    void Start()
    {
        theRB.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        turnInput = Input.GetAxis("Horizontal");


        if (grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }

        leftfrontwheel.localRotation = Quaternion.Euler(leftfrontwheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftfrontwheel.localRotation.eulerAngles.z);
        rightfrontwheel.localRotation = Quaternion.Euler(rightfrontwheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightfrontwheel.localRotation.eulerAngles.z);


        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 1000f;

        }
        else if(Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 1000f;
        }

        transform.position = theRB.transform.position;
    }

    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLenght, whatIsGround))
        {
            grounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        emissionRate = 0;

        if (grounded)
        {
            if (Mathf.Abs(speedInput) > 0)
            {
                theRB.AddForce(transform.forward * speedInput);

                emissionRate = maxEmission;
            }
        }
        else
        {
            theRB.AddForce(Vector3.up * -gravityForce * 100f);
        }
        
        
    }
}
