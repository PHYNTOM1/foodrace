using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public Rigidbody theRB;

    public float forwardAccel = 1f, reverseAccel = 0.66f, maxSpeed = 8f, turnStrength = 120, gravityForce = 10f;

    public float speedInput, turnInput;

    private bool grounded;
    private bool drifting = false;
    private Vector3 driftingStance;
    private float driftDir = 0f;
    public int driftForce = 0;

    public float driftInput = 0f;

    public LayerMask whatIsGround;
    public float groundRayLenght = .5f;
    public Transform groundRayPoint;

    public Transform leftfrontwheel, rightfrontwheel;
    public float maxWheelTurn = 25f;

    private ParticleSystem exhaust;
    private ParticleSystem[] drift;
    private bool exhaustEmitting;
    private bool driftEmitting;

    public KartStats kartStats;


    void Start()
    {
        theRB.transform.parent = null;
        exhaust = GameObject.Find("carSmoke").GetComponent<ParticleSystem>();
        GameObject driftEffects = GameObject.Find("driftEffects");
        drift = driftEffects.GetComponentsInChildren<ParticleSystem>();

        maxSpeed = kartStats.topSpeed;
        forwardAccel = kartStats.acceleration;
        reverseAccel = forwardAccel * 2 / 3;
        turnStrength = kartStats.handling * 60f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            drifting = true;
        }
        else
        {
            drifting = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            driftForce = (int)Input.GetAxisRaw("Horizontal");
        }

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
        {
            turnInput = Input.GetAxis("Horizontal");

            driftInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            driftInput = 0f;

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


        if (Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                if (speedInput < maxSpeed * 1000f && speedInput >= maxSpeed * -1000f)
                {
                    speedInput += forwardAccel * maxSpeed * 20f;
                }
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                if (speedInput > 0 && speedInput <= maxSpeed * 1000f)
                {
                    speedInput -= forwardAccel * maxSpeed * 20f;
                }
                else if (speedInput >= maxSpeed * -750f && speedInput <= 0f)
                {
                    speedInput -= forwardAccel * maxSpeed * 5f;
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


        if (grounded)
        {
            if (Input.GetAxisRaw("Vertical") == -1)
            {
                turnInput *= -1;
            }
            float _turnInput = turnInput * turnStrength * Time.deltaTime;

            if (drifting)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * 0.55f, 0f));
            }
            else
            {
                if (speedInput <= 5000 && speedInput >= 0 || speedInput >= -5000 && speedInput <= 0)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * 0.3f, 0f));
                }
                else if (speedInput <= 7000 && speedInput >= 0 || speedInput >= -7000 && speedInput <= 0)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * 0.35f, 0f));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * 0.4f, 0f));
                }
            }

        }

        leftfrontwheel.localRotation = Quaternion.Euler(leftfrontwheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftfrontwheel.localRotation.eulerAngles.z);
        rightfrontwheel.localRotation = Quaternion.Euler(rightfrontwheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightfrontwheel.localRotation.eulerAngles.z);

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

        exhaustEmitting = false;
        driftEmitting = false;

        if (grounded)
        {
            if (Mathf.Abs(speedInput) > 0)
            {
                if (!drifting)
                {
                    theRB.AddForce((transform.forward * speedInput) + (transform.right * turnInput * turnStrength * (speedInput / (maxSpeed * 1000f)) * 10f));
                }
                else
                {
                    theRB.AddForce((transform.forward * speedInput * 1.2f) + (transform.right * -driftForce * 6000f));
                    driftEmitting = true;
                }

                exhaustEmitting = true;
            }
        }
        else
        {
            theRB.AddForce((Vector3.up * -gravityForce * 700f) + (transform.forward * speedInput * 0.8f) + (transform.right * turnInput * turnStrength * (speedInput / (maxSpeed * 1000f)) * 10f * 0.8f)); ;
        }

        ParticleSystem.EmissionModule em = exhaust.emission;
        em.enabled = exhaustEmitting;

        foreach (ParticleSystem dr in drift)
        {
            ParticleSystem.EmissionModule ift = dr.emission;
            ift.enabled = driftEmitting;
        }
    }
}
