﻿using JetBrains.Annotations;
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
    public LayerMask speedGround;
    public float groundRayLenght = .5f;
    public Transform groundRayPoint;
    public bool speeding = false;
    public float speedMult = 1f;

    public Transform leftfrontwheel, rightfrontwheel;
    public float maxWheelTurn = 25f;

    private ParticleSystem exhaust;
    private ParticleSystem[] drift;
    private bool exhaustEmitting;
    private bool driftEmitting;

    public KartStats kartStats;
    public bool isPlayer = true;

    public AIExtra aiExtra;


    void Start()
    {
        theRB.transform.parent = null;
        exhaust = GameObject.Find(gameObject.name + "/Normal/Mesh/Effects/carSmoke").GetComponent<ParticleSystem>();
        GameObject driftEffects = GameObject.Find(gameObject.name + "/Normal/Mesh/Effects/driftEffects");
        drift = driftEffects.GetComponentsInChildren<ParticleSystem>();

        maxSpeed = kartStats.topSpeed;
        forwardAccel = kartStats.acceleration;
        reverseAccel = forwardAccel * 2 / 3;
        turnStrength = kartStats.handling * 60f;
    }

    void Update()
    {
        bool _driftInput;
        bool _driftInputDown;
        float _horAxis;
        float _horAxisRaw;
        float _verAxis;
        float _verAxisRaw;

        if (isPlayer)
        {
             _driftInput = Input.GetKey(KeyCode.LeftShift);
             _driftInputDown = Input.GetKeyDown(KeyCode.LeftShift);
            _horAxis = Input.GetAxis("Horizontal");
            _horAxisRaw = Input.GetAxisRaw("Horizontal");
            _verAxis = Input.GetAxis("Vertical");
            _verAxisRaw = Input.GetAxisRaw("Vertical");
        }
        else
        {
            if (aiExtra == null)
            {
                aiExtra = gameObject.GetComponent<AIExtra>();
            }
            _driftInput = aiExtra.DriftOutput();
             _driftInputDown = aiExtra.DriftDownOutput();
            _horAxis = aiExtra.HorAxis();
            _horAxisRaw = aiExtra.HorAxisRaw();
             _verAxis = aiExtra.VerAxis();
             _verAxisRaw = aiExtra.VerAxisRaw();
        }

        if (_driftInput && speedInput >= maxSpeed * 750f)
        {
            drifting = true;
        }
        else
        {
            drifting = false;
        }

        if (_driftInputDown)
        {
            driftForce = (int)_horAxisRaw;
        }

        if (_horAxisRaw == 1 || _horAxisRaw == -1)
        {
            turnInput = _horAxis;

            driftInput = _horAxisRaw;
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


        if (speeding)
        {
            speedMult = 1.2f;
        }
        else
        {
            speedMult = 1f;
        }

        if (_verAxisRaw == 1 || _verAxisRaw == -1)
        {
            if (_verAxis > 0)
            {
                if (speedInput < maxSpeed * 1000f && speedInput >= maxSpeed * -1000f)
                {
                    speedInput += forwardAccel * maxSpeed * speedMult * 20f;
                }
            }
            else if (_verAxis < 0)
            {
                if (speedInput > 0 && speedInput <= maxSpeed * 1000f)
                {
                    speedInput -= forwardAccel * maxSpeed * 10f;
                }
                else if (speedInput >= maxSpeed * -250f && speedInput <= 0f)
                {
                    speedInput -= forwardAccel * maxSpeed * 5f;
                }
            }
        }
        else
        {
            if (speedInput <= maxSpeed * 2000f && speedInput >= maxSpeed * 20f)
            {
                speedInput -= 50f;
            }
            else if (speedInput >= -maxSpeed * 2000f && speedInput <= -maxSpeed * 20f)
            {
                speedInput += 25f;
            }
            else
            {
                speedInput = 0;
            }
        }

        if (grounded)
        {
            
            if (_verAxisRaw == -1)
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
            speeding = false;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        else if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLenght, speedGround))
        {
            grounded = true;
            speeding = true;

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
                    theRB.AddForce((transform.forward * speedInput * 0.75f) + (transform.right * -driftForce * 6000f));
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
