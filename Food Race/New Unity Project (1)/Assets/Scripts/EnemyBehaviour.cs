using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class EnemyBehaviour : MonoBehaviour
{  
    public enum EnemyType
    {
        walker,
        flyer,
        turret
    }
    public EnemyType eType;

    public bool spotted = false;
    public float spottedCounter = 1.5f;
    [SerializeField]
    public float spottedCounterReal = 0f;
    public Image spotIcon;
    public Image oooIcon;
    public int maxHP = 3;
    public int currHP = 3;

    public float moveSpeed = 3f;

    public float atkSpeed = 1f;
    [SerializeField]
    private float realAtkSpeed = 0f;
    public GameObject enemyBullet;
    public float left = 2f;
    public bool outOfOrder = false;
    public float oooCD = 2f;
    [SerializeField]
    private float oooCDReal = 0f;

    public Rigidbody rb;
    public GameObject player;
    public Vector3 targetPos;
    public List<Transform> targetPosT = new List<Transform>();
    public int targetPosTPos = 0;
    public Vector3 distPos = new Vector3(0f, 0f, 0f);
    public Animator anim;
    public GameObject smokePoof;
    public VisualEffect[] destroyed;


    void Start()
    {
        targetPos = gameObject.transform.position;
        currHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();

        if (eType != EnemyType.turret)
        {
            anim = GetComponent<Animator>();

            for (int i = 0; i < Random.Range(2, 6); i++)
            {
                Vector3 currPos = transform.position;
                currPos.x += Random.Range(-20, 20);
                currPos.z += Random.Range(-20, 20);

                GameObject g = new GameObject();
                g.transform.position = currPos;

                targetPosT.Add(g.transform);
                //Destroy(g);
            }
        }
        else if (eType == EnemyType.turret)
        {
            oooIcon.enabled = false;
        }

        if (eType == EnemyType.walker)
        {
            targetPos = targetPosT[Random.Range(0, targetPosT.Count)].position;
            transform.LookAt(transform.position + ((transform.position - targetPos)));
        }

        spotIcon.enabled = false;
    }

    void Update()
    {
        switch (eType)
        {
            case EnemyType.walker:

                if (spotted)
                {
                    transform.LookAt(transform.position + ((transform.position - player.transform.position)));
                }
                else
                {
                    Move();
                }
                break;
            case EnemyType.flyer:
                
                if (spotted)
                {
                    if (Vector3.Distance(transform.position, player.transform.position) < 5f)
                    {
                        if (left == 2f)
                        {
                            player.GetComponent<CartController>().flyerCount++;
                            left = 0f;
                        }
                        CircleAroundPlayer();
                    }
                    else
                    {
                        MoveToPlayer();
                    }
                }
                else
                {
                    //Move();
                }
                break;
            case EnemyType.turret:

                if (outOfOrder == false)
                {
                    if (spotted)
                    {
                        DoShooting();
                    }

                    if (realAtkSpeed > 0)
                    {
                        realAtkSpeed -= Time.deltaTime;
                    }
                }
                else
                {
                    if (oooCDReal > 0)
                    {
                        oooCDReal -= Time.deltaTime;
                    }
                    else
                    {
                        oooIcon.enabled = false;
                        currHP = maxHP;
                        oooCDReal = oooCD;
                        outOfOrder = false;
                    }
                }
                break;
        }

        if (spottedCounterReal > 0)
        {
            spottedCounterReal -= Time.deltaTime;
        }
        else if (spotted)
        {
            spotIcon.enabled = false;
        }
    }

    public void Move()
    {
        //calculate player position + forward by certain distance, save this position and move there, then turn to face player     

        if (Vector3.Distance(transform.position, targetPos) > 6f)
        {
            rb.AddForce(-transform.forward * moveSpeed, ForceMode.Force);
        }
        else
        {
            rb.velocity = Vector3.zero;
            //outOfOrder = false;
            NextMovePoint();
        }
    }

    public void NextMovePoint()
    {
        targetPosTPos++;

        if (targetPosTPos >= targetPosT.Count)
        {
            targetPosTPos = 0;
        }

        targetPos = targetPosT[targetPosTPos].position;

        transform.LookAt(transform.position + ((transform.position - targetPos)));

        //if (outOfOrder == false)
        //{
        //    outOfOrder = true;
        //    targetPos = player.transform.position + (player.transform.forward * 70f);
        //    transform.rotation = Quaternion.FromToRotation(-transform.forward, (targetPos - gameObject.transform.position));
        //}
    }

    public void MoveToPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        GetComponent<Rigidbody>().MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        /*
        targetPos = player.transform.position;
        transform.rotation = Quaternion.FromToRotation(-transform.forward, (targetPos - transform.position));
        //fly straight to the player,
        //if in range, start circling around him
        //decrease player topSpeed and acceleration
        rb.AddForce(-transform.forward * moveSpeed, ForceMode.Force);
        */
    }

    public void CircleAroundPlayer()
    {
        Vector3 playerPos = player.gameObject.transform.position;
        realAtkSpeed += Time.deltaTime * atkSpeed;

        float x = Mathf.Cos(realAtkSpeed);
        float y = 0;
        float z = Mathf.Sin(realAtkSpeed);
        transform.position = playerPos + (player.transform.forward * oooCD/4) + (new Vector3(x, y, z).normalized * oooCD);

        transform.LookAt(transform.position + ((transform.position - playerPos).normalized * oooCD));
        /*
        if (transform.position <= playerPos)
        {
        }
        else
        {
            transform.LookAt(playerPos - ((transform.position - playerPos).normalized * oooCD));
        }
        
        transform.position = (playerPos + ((transform.position - playerPos).normalized * oooCD));

        distPos = player.gameObject.transform.TransformDirection(Vector3.down);
        transform.RotateAround(playerPos, distPos, atkSpeed * Time.deltaTime);
                
        Vector3 lookPos = playerPos - gameObject.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(-lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * atkSpeed * 10f);
        //transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        */
    }

    public void DoShooting()
    {
        if (realAtkSpeed <= 0)
        {
            GameObject eB = Instantiate(enemyBullet, (this.gameObject.transform.position - (this.gameObject.transform.forward.normalized * 3f) + (this.gameObject.transform.right.normalized * left)), Quaternion.Euler(90f, this.gameObject.transform.eulerAngles.y + 180f, 0f));

            left = -left;

            eB.transform.parent = null;
            realAtkSpeed = atkSpeed;
        }
    }

    public void GetDamaged(int d)
    {
        currHP -= d;

        if (currHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        switch (eType)
        {
            case EnemyType.walker:

                //DO ANIMATION AND PARTICLES
                GameObject sp = Instantiate(smokePoof, gameObject.transform.position + new Vector3(0f, 0.2f, 0f), Quaternion.identity);
                Destroy(sp, 1f);
                Destroy(this.gameObject, 0.15f);
                this.enabled = false;
                break;
            case EnemyType.flyer:

                //DO ANIMATION AND PARTICLES
                GameObject sp2 = Instantiate(smokePoof, gameObject.transform.position + new Vector3(0f, 0.2f, 0f), Quaternion.identity);
                Destroy(sp2, 1f);
                player.GetComponent<CartController>().flyerCount--;
                Destroy(this.gameObject, 0.15f);
                this.enabled = false;
                break;
            case EnemyType.turret:

                foreach (VisualEffect v in destroyed)
                {
                    v.Play();
                }

                oooIcon.enabled = true;
                oooCDReal = oooCD;
                outOfOrder = true;
                break;
        }
    }


    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "WPCollider")
        {
            spotted = true;
            spotIcon.enabled = true;
            spottedCounterReal = spottedCounter;

            if (eType != EnemyType.turret)
            {
                anim.SetTrigger("Spotted");
            }

            /*
            if (eType == EnemyType.walker)
            {
                if (outOfOrder == false)
                {
                    outOfOrder = true;
                    targetPos = player.transform.position + (player.transform.forward * 70f);
                    transform.rotation = Quaternion.FromToRotation(-transform.forward, (targetPos - gameObject.transform.position));
                }
            }
            else 
            */
            if (eType == EnemyType.flyer)
            {
                targetPos = player.gameObject.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.name == "WPCollider")
        {
            spotted = false;

            if (eType == EnemyType.walker)
            {
                transform.LookAt(transform.position + ((transform.position - targetPos)));
            }
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.gameObject.name == "WPCollider")
        {
            if (eType == EnemyType.walker)
            {
                coll.collider.GetComponentInParent<CartController>().GetStunned();
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawLine(gameObject.transform.position, targetPos);
    }
}
