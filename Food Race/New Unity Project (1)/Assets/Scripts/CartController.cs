using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public Rigidbody theRB;

    public float forwardAccel = 1f, reverseAccel = 0.66f, maxSpeed = 8f, turnStrength = 120, gravityForce = 10f;

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

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
        {
            turnInput = Input.GetAxis("Horizontal");
        }
        else
        {
            if (turnInput <= 1f && turnInput >= 0.1f)
            {
                turnInput -= 0.1f;
            }
            else if (turnInput >= -1f && turnInput <= -0.1f)
            {
                turnInput += 0.1f;
            }
            else
            {
                turnInput = 0;
            }
        }

        if (grounded)
        {
            if (speedInput <= 3000 && speedInput >= 0 || speedInput >= -3000 && speedInput <= 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * 0.1f, 0f));
            }
            else if (speedInput <= 5000 && speedInput >= 0 || speedInput >= -5000 && speedInput <= 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * 0.2f, 0f));
            }
            else if (speedInput <= 7000 && speedInput >= 0 || speedInput >= -7000 && speedInput <= 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * 0.3f, 0f));
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * 0.4f, 0f));
            }
        }

        leftfrontwheel.localRotation = Quaternion.Euler(leftfrontwheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftfrontwheel.localRotation.eulerAngles.z);
        rightfrontwheel.localRotation = Quaternion.Euler(rightfrontwheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightfrontwheel.localRotation.eulerAngles.z);


        //speedInput = 0f;

        if (Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                if (speedInput < maxSpeed * 1000f && speedInput >= 0f)
                {
                    speedInput += forwardAccel * maxSpeed * 10f;
                }
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                if (speedInput > -maxSpeed * 1000f && speedInput <= 0f)
                {
                    speedInput -= forwardAccel * maxSpeed * 20f;
                }
            }
        }
        else
        {
            if (speedInput <= maxSpeed * 1000f && speedInput >= maxSpeed * 50f)
            {
                speedInput -= 50f;
            }
            else if (speedInput >= -maxSpeed * 1000f && speedInput <= -maxSpeed * 50f)
            {
                speedInput += 50f;
            }
            else
            {
                speedInput = 0;
            }
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
