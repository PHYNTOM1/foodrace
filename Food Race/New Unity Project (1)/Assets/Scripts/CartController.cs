using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Security;
using UnityEngine;
using UnityEngine.VFX;

public class CartController : MonoBehaviour
{
    public Rigidbody theRB;
    public Transform sp;

    public float forwardAccel = 1f, reverseAccel = 0.66f, maxSpeed = 8f, turnStrength = 120, gravityForce = 10f;
    public float speedInput, turnInput;

    private bool grounded;
    public bool drifting = false;
    public int driftForce = 0;
    public float driftInput = 0f;
    public float driftTimer = 0f;
    public float driftBoostTimer = 0.66f;
    public float driftBoostTimer2 = 1f;
    public int driftStage = 0;

    public LayerMask whatIsGround;
    public LayerMask speedGround;
    public LayerMask offroadGround;
    public float groundRayLenght = .5f;
    public Transform groundRayPoint;
    public bool speeding = false;
    public bool slowed = false;
    public float speedMult = 1f;

    public Transform leftfrontwheel, rightfrontwheel;
    public float maxWheelTurn = 25f;

    public ParticleSystem exhaust;
    public ParticleSystem[] drift;
    public ParticleSystem[] boost;
    public TrailRenderer[] skid;
    public ParticleSystem lines;
    private bool exhaustEmitting;
    private bool driftEmitting;
    private bool boostEmitting;
    public float boostEmitTime = 0f;
    public float boostCounter = 2f;
    public float boostCounterReal = 0f;

    public KartStats kartStats;
    public SoundManagement sm;
    //public SkillHolder sh;
    public CameraController camC;
    public bool isPlayer = false;
    public bool notRacing = false;
    public bool inMinigame = false;

    bool _driftInput;
    float _horAxis;
    float _horAxisRaw;
    float _verAxis;
    float _verAxisRaw;

    public bool stunned = false;
    public float stunTimer = 1f;
    [SerializeField]
    private float stunTimerReal = 0f;

    public int flyerCount = 0;
    public Animator anim;
    public VisualEffect[] driftIgnites;
    public VisualEffect[] driftIgnites2;
    public ParticleSystem[] driftFlames;
    public ParticleSystem[] driftFlames2;
    public ParticleSystem[] boostFlames;
    public Camera cam;

    void Start()
    {
        PlacementManagement.Instance.CallOnAwake();
        sm = PlacementManagement.Instance.gameObject.GetComponent<SoundManagement>();

        camC = FindObjectOfType<CameraController>();
        cam = camC.gameObject.GetComponent<Camera>();
        theRB.transform.parent = null;  
        lines = GameObject.FindGameObjectWithTag("Lines").GetComponent<ParticleSystem>();
        //sh = GetComponent<SkillHolder>();
        anim = GetComponent<Animator>();

        maxSpeed = kartStats.topSpeed;
        forwardAccel = kartStats.acceleration;
        reverseAccel = forwardAccel * 2 / 3;
        turnStrength = kartStats.handling * 60f;

        sp = GameObject.Find("StartPoint").transform;
        sm.Play("engine");
        sm.TogglePause("engine");
    }

    void Update()
    {
        if (sm == null)
        {
            sm = PlacementManagement.Instance.gameObject.GetComponent<SoundManagement>();
            sm.Play("engine");
            sm.TogglePause("engine");
        }

        if (notRacing)
        {

        }
        else if (stunned)
        {
            //this mode when "stunned"
            if (stunTimerReal <= stunTimer)
            {
                stunTimerReal += Time.deltaTime;
            }
            else
            {
                stunned = false;
                stunTimerReal = 0f;
            }
        }
        else
        {
            /*
            if (!isPlayer)
            {
                if (sh.allSkills.ContainsKey(1))
                {
                    sh.ActivateATKSkill();
                }
                else if (sh.allSkills.ContainsKey(2))
                {
                    sh.ActivateDEFSkill();
                }
            }
            */

            if (_driftInput && speedInput >= maxSpeed * 750f)
            {
                drifting = true;
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
            else if (slowed)
            {
                speedMult = 0.4f;
            }
            else
            {
                speedMult = 1f;
            }

            if (flyerCount > 0)
            {
                speedMult -= 0.2f;
            }

            if (_verAxisRaw == 1 || _verAxisRaw == -1)
            {
                
                if (sm.IsPlaying("engine") == false)
                {
                    sm.TogglePause("engine", false);
                }
                
                if (_verAxis > 0)
                {
                    if (speedInput > maxSpeed * 1005f * speedMult)
                    {
                        speedInput -= forwardAccel * maxSpeed * 22.5f * speedMult;
                    }
                    else if (speedInput < maxSpeed * 1000f * speedMult && speedInput >= maxSpeed * -1000f * speedMult)
                    {
                        speedInput += forwardAccel * maxSpeed * 20f * speedMult;
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
                
                if (sm.IsPlaying("engine") == true)
                {
                    sm.TogglePause("engine", true);
                }
                
                if (speedInput <= maxSpeed * 2000f && speedInput >= maxSpeed * 90f)
                {
                    speedInput -= 150f;
                }
                else if (speedInput >= -maxSpeed * 2000f && speedInput <= -maxSpeed * 90f)
                {
                    speedInput += 75f;
                }
                else
                {
                    speedInput = 0;
                }
            }

            //anim.SetBool("Grounded", grounded);
            if (grounded)
            {
                /*
                if (_verAxisRaw == -1 && speedInput < 0)
                {
                    turnInput *= -1;
                }
                */

                float _turnInput = turnInput * turnStrength * Time.deltaTime;

                if (drifting)
                {
                    
                    if (sm.IsPlaying("drifting") == false && driftStage < 2)
                    {
                        sm.Play("drifting");
                    }
                    else if (sm.IsPlaying("drifting") == true && driftStage >= 2)
                    {
                        sm.Stop("drifting");
                    }                                        

                    if (camC.sSpeed < 14f)
                    {
                        camC.sSpeed += Time.deltaTime * 5f;
                    }

                    if (_horAxisRaw != driftForce)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * 0.3f, 0f));    //0.55f
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _turnInput * 0.6f, 0f));    //0.55f
                    }

                    driftTimer += Time.deltaTime;

                    if (driftTimer >= driftBoostTimer && driftStage == 0)
                    {
                        sm.PlayOneShot("check3");
                        sm.PlayOneShot("clickcheckswoosh");

                        foreach (VisualEffect v in driftIgnites)
                        {
                            v.Play();
                        }
                        driftStage = 1;
                        driftTimer = 0;
                    }
                    else if (driftTimer >= driftBoostTimer2 && driftStage == 1)
                    {
                        sm.PlayOneShot("check3");
                        sm.PlayOneShot("check7");
                        sm.PlayOneShot("clickcheckswoosh");

                        foreach (VisualEffect v in driftIgnites2)
                        {
                            v.Play();
                        }
                        driftStage = 2;
                        driftTimer = 0;
                    }                 
                }
                else
                {
                    
                    if (sm.IsPlaying("drifting") == true)
                    {
                        sm.Stop("drifting");
                    }
                    

                    if (camC.sSpeed > 11f)
                    {
                        camC.sSpeed -= Time.deltaTime * 5f;
                    }

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
            //JUST FOR TESTING :))))) DELETE ME LATER
            else
            {
                if (theRB.position.y <= -10f)
                {
                    ResetCart(true);
                }
            }

            if (boostCounterReal >= 0)
            {
                boostCounterReal -= Time.deltaTime;
            }
            else
            {
                if (boostEmitting != false)
                {
                    boostEmitting = false;
                }
            }
        }

    }

    private void FixedUpdate()
    {
        //leftfrontwheel.localRotation = Quaternion.Euler(leftfrontwheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftfrontwheel.localRotation.eulerAngles.z);
        //rightfrontwheel.localRotation = Quaternion.Euler(rightfrontwheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightfrontwheel.localRotation.eulerAngles.z);

        transform.position = theRB.transform.position;

        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLenght, whatIsGround))
        {
            grounded = true;
            speeding = false;
            slowed = false;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        else if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLenght, offroadGround))
        {
            grounded = true;
            speeding = false;
            slowed = true;

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
                        theRB.AddForce((transform.forward * speedInput * 0.75f) + (transform.right * -1 * 5000f));
                    }
                    else
                    {
                        theRB.AddForce((transform.forward * speedInput * 0.75f) + (transform.right * -driftForce * 5000f));
                    }
                    driftEmitting = true;
                }

                exhaustEmitting = true;
            }
        }
        else
        {
            theRB.AddForce((Vector3.up * -gravityForce * 700f) + (transform.forward * speedInput * 0.8f) + (transform.right * turnInput * turnStrength * (speedInput / (maxSpeed * 1000f)) * 10f * 0.8f));
        }

        if (boostEmitting == true && cam.fieldOfView < 95f)
        {
            cam.fieldOfView += Time.fixedDeltaTime * 10f;
        }
        else if (boostEmitting == false && cam.fieldOfView > 70f)
        {
            cam.fieldOfView -= Time.fixedDeltaTime * 6f;
        }


        ParticleSystem.EmissionModule em = exhaust.emission;
        em.enabled = exhaustEmitting;

        foreach (ParticleSystem dr in drift)
        {
            ParticleSystem.EmissionModule ift = dr.emission;
            ift.enabled = driftEmitting;
        }

        foreach (TrailRenderer tr in skid)
        {
            tr.emitting = driftEmitting;
        }

        foreach (ParticleSystem bs in boost)
        {
            ParticleSystem.EmissionModule oot = bs.emission;
            oot.enabled = boostEmitting;

            if (boostEmitting)
            {
                ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams
                {
                    //startLifetime = 3f
                };
                bs.Emit(emitOverride, 1);
            }
        }

        foreach (ParticleSystem bf in boostFlames)
        {
            ParticleSystem.EmissionModule bfe = bf.emission;
            bfe.enabled = boostEmitting;
        }

        ParticleSystem.EmissionModule ln = lines.emission;
        ln.enabled = boostEmitting;

        if (driftStage == 1)
        {
            foreach (ParticleSystem df in driftFlames)
            {
                ParticleSystem.EmissionModule dfe = df.emission;
               dfe.enabled = true;
            }
        }
        else if (driftStage == 2)
        {
            foreach (ParticleSystem df2 in driftFlames2)
            {
                ParticleSystem.EmissionModule df2e = df2.emission;
                df2e.enabled = true;
            }
        }
        else
        {
            foreach (ParticleSystem df in driftFlames)
            {
                ParticleSystem.EmissionModule dfe = df.emission;
                dfe.enabled = false;
            }
            foreach (ParticleSystem df2 in driftFlames2)
            {
                ParticleSystem.EmissionModule df2e = df2.emission;
                df2e.enabled = false;
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
        sm.Play("boostair3");
        sm.Play("boostair");
        sm.Play("boostair2", 0.15f);
        speedInput += 3500 + (1500 * stage);

        driftStage = 0;
        boostEmitting = true;
        boostCounterReal = boostCounter;
    }

    /*
    private IEnumerator WaitNSetBoostPSFalse()
    {
        yield return new WaitForSecondsRealtime(boostEmitTime);
        boostEmitting = false;
    }
    */

    public void GetStunned()
    {
        sm.PlayOneShot("wheeeoow");
        sm.PlayOneShot("punch");
        anim.SetTrigger("Stun");
        speedInput = 0f;
        stunned = true;
        stunTimerReal = 0f;
        driftStage = 0;
    }

    public void ResetCart(bool d)
    {
        theRB.velocity = Vector3.zero;
        theRB.rotation = Quaternion.identity;
        speedInput = 0f;

        if (d == true)
        {
            gameObject.transform.eulerAngles = Vector3.zero;
            theRB.position = sp.position;
            EnemyRespawning er = FindObjectOfType<EnemyRespawning>();
            er.RespawnEnemies();
            GetComponent<PlayerShooting>().overheatValue = 0f;
            GetComponent<LapTracker>().ResetAll();
            GetComponent<RoundTimer>().RoundTimerReset();
            sm.PlayOneShot("error");
        }
        else
        {
            gameObject.transform.eulerAngles = new Vector3(0f, this.gameObject.transform.eulerAngles.y, 0f);
            theRB.position += new Vector3(0f, 2f, 0f);
        }
    }
}
