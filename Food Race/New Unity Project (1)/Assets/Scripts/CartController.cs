using Google.Protobuf.WellKnownTypes;
using JetBrains.Annotations;
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
    public int driftForce = 0;
    public float driftInput = 0f;
    public float driftTimer = 0f;
    public float driftBoostTimer = 1f;
    public int driftStage = 0;

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
    private ParticleSystem[] boost;
    private bool exhaustEmitting;
    private bool driftEmitting;
    private bool boostEmitting;
    public float boostEmitTime = 0f;

    public KartStats kartStats;
    public SoundManagement sm;
    public bool isPlayer = true;
    public bool notRacing = false;
    public bool inMinigame = false;

    bool _driftInput;
    float _horAxis;
    float _horAxisRaw;
    float _verAxis;
    float _verAxisRaw;

    void Start()
    {
        theRB.transform.parent = null;
        exhaust = GameObject.Find(gameObject.name + "/Normal/Mesh/Effects/carSmoke").GetComponent<ParticleSystem>();
        GameObject driftEffects = GameObject.Find(gameObject.name + "/Normal/Mesh/Effects/driftEffects");
        drift = driftEffects.GetComponentsInChildren<ParticleSystem>();
        GameObject boostEffects = GameObject.Find(gameObject.name + "/Normal/Mesh/Effects/boostEffects");
        boost = boostEffects.GetComponentsInChildren<ParticleSystem>();
        sm = FindObjectOfType<SoundManagement>();

        maxSpeed = kartStats.topSpeed;
        forwardAccel = kartStats.acceleration;
        reverseAccel = forwardAccel * 2 / 3;
        turnStrength = kartStats.handling * 60f;
    }

    void Update()
    {
        if (notRacing)
        {
            return;
        }

        if (inMinigame)
        {
            //TODO: ???
            //do minigame, handle inputs for minigame here. Control minigame in own script, through methods from here
        }
        else
        {
            /*
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
            */

            if (_driftInput && speedInput >= maxSpeed * 750f)
            {
                drifting = true;
                sm.Play("Drifting");
                if (driftForce == 0)
                {
                    driftForce = (int)_horAxisRaw;
                }
            }
            else
            {
                drifting = false;
                driftForce = 0;
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
                sm.Play("EngineDriving");

                if (_verAxis > 0)
                {
                    if (speedInput > maxSpeed * 1000f)
                    {
                        speedInput -= forwardAccel * maxSpeed * 15f;
                    }
                    else if (speedInput < maxSpeed * 1000f && speedInput >= maxSpeed * -1000f)
                    {
                        speedInput += forwardAccel * maxSpeed * speedMult * 20f;
                    }
                }
                else if (_verAxis < 0)
                {
                    if (speedInput > 0 && speedInput <= maxSpeed * 2000f)
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

                if (_verAxisRaw == -1 && speedInput < 0)
                {
                    turnInput *= -1;
                }

                float _turnInput = turnInput * turnStrength * Time.deltaTime;

                if (drifting)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * 0.5f, 0f));    //0.55f

                    driftTimer += 1 * Time.deltaTime;

                    if (driftTimer >= driftBoostTimer && driftStage < 2)
                    {
                        driftStage++;
                        driftTimer = 0;
                    }
                }
                else
                {
                    //for testing purposes commented, but works velly nice without :)
                    /*
                    if (speedInput <= maxSpeed * 500f && speedInput >= 0 || speedInput >= maxSpeed * -500f && speedInput <= 0)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * 0.3f, 0f));     //0.3f
                    }
                    else if (speedInput <= maxSpeed * 950f && speedInput >= 0 || speedInput >= maxSpeed * -950f && speedInput <= 0)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * 0.6f, 0f));    //0.35f
                    }
                    else
                    {
                    */
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * 0.6f, 0f));     //0.4f
                                                                                                                                        //}
                    driftTimer = 0f;
                }
            }
        }

    }

    private void FixedUpdate()
    {
        leftfrontwheel.localRotation = Quaternion.Euler(leftfrontwheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftfrontwheel.localRotation.eulerAngles.z);
        rightfrontwheel.localRotation = Quaternion.Euler(rightfrontwheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightfrontwheel.localRotation.eulerAngles.z);

        transform.position = theRB.transform.position;

        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLenght, whatIsGround))
        {
            grounded = true;
            speeding = false;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        /*
        else if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLenght, speedGround))
        {
            grounded = true;
            speeding = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        */

        exhaustEmitting = false;
        driftEmitting = false;

        if (grounded)
        {
            if (Mathf.Abs(speedInput) > 0)
            {
                if (!drifting)
                {
                    if (driftStage > 0)
                    {
                        GetBoosted(driftStage);
                    }

                    theRB.AddForce((transform.forward * speedInput) + (transform.right * turnInput * turnStrength * (speedInput / (maxSpeed * 1000f)) * 10f));
                }
                else
                {
                    if (driftForce == 0)
                    {
                        theRB.AddForce((transform.forward * speedInput * 0.75f) + (transform.right * -1 * 6000f));
                    }
                    else
                    {
                        theRB.AddForce((transform.forward * speedInput * 0.75f) + (transform.right * -driftForce * 6000f));
                    }
//                    driftEmitting = true;
                }

//                exhaustEmitting = true;
            }
        }
        else
        {
            theRB.AddForce((Vector3.up * -gravityForce * 700f) + (transform.forward * speedInput * 0.8f) + (transform.right * turnInput * turnStrength * (speedInput / (maxSpeed * 1000f)) * 10f * 0.8f));
        }

        ParticleSystem.EmissionModule em = exhaust.emission;
        em.enabled = exhaustEmitting;

        foreach (ParticleSystem dr in drift)
        {
            ParticleSystem.EmissionModule ift = dr.emission;
            ift.enabled = driftEmitting;
        }

        foreach (ParticleSystem bs in boost)
        {
            ParticleSystem.EmissionModule oot = bs.emission;
            oot.enabled = boostEmitting;

            if (boostEmitting)
            {
                ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams
                {
                    startLifetime = 3f
                };
                bs.Emit(emitOverride, 1);
            }
        }
    }

    public void SteerInput(float hor)
    {
        _horAxis = hor;

        if (_horAxis > 0)
        {
            _horAxisRaw = 1;
        }
        else if (_horAxis < 0)
        {
            _horAxisRaw = -1;
        }
        else
        {
            _horAxisRaw = 0;
        }
    }

    public void AccelerationInput(float ver)
    {
        _verAxis = ver;

        if (_verAxis > 0)
        {
            _verAxisRaw = 1;
        }
        else if (_verAxis < 0)
        {
            _verAxisRaw = -1;
        }
        else
        {
            _verAxisRaw = 0;
        }
    }

    public void DriftInput(bool drift)
    {
        _driftInput = drift;
    }

    public void GetBoosted(int stage)
    {
        speedInput += 7000 + (2500 * stage);

        driftStage = 0;
//        boostEmitting = true;
//        StartCoroutine("WaitNSetBoostPSFalse");
    }

    private IEnumerator WaitNSetBoostPSFalse()
    {
        yield return new WaitForSecondsRealtime(boostEmitTime);
        boostEmitting = false;
    }

}
