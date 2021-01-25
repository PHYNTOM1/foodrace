using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

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
    public Vector3 distPos = new Vector3(0f, 0f, 0f);


    void Start()
    {
        targetPos = gameObject.transform.position;
        currHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        switch (eType)
        {
            case EnemyType.walker:
                
                Move();
                break;
            case EnemyType.flyer:
                
                if (spotted)
                {
                    if (Vector3.Distance(gameObject.transform.position, targetPos) < 6)
                    {
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
                        currHP = maxHP;
                        oooCDReal = oooCD;
                        outOfOrder = false;
                    }
                }
                break;
        }
    }

    public void Move()
    {
        //calculate player position + forward by certain distance, save this position and move there, then turn to face player     

        if (Vector3.Distance(transform.position, targetPos) > 0.5f)
        {
            rb.AddForce(-transform.forward * moveSpeed, ForceMode.Force);
        }
        else
        {
            rb.velocity = Vector3.zero;
            outOfOrder = false;
        }
    }

    public void MoveToPlayer()
    {
        targetPos = player.transform.position;
        transform.rotation = Quaternion.FromToRotation(-transform.forward, (targetPos - gameObject.transform.position));
        //fly straight to the player,
        //if in range, start circling around him
        //decrease player topSpeed and acceleration
        rb.AddForce(-transform.forward * moveSpeed, ForceMode.Force);

    }

    public void CircleAroundPlayer()
    {
        Vector3 playerPos = player.gameObject.transform.position;

        transform.position = (playerPos + ((gameObject.transform.position - playerPos).normalized * oooCD));
        
        transform.RotateAround(playerPos, Vector3.down, atkSpeed * Time.deltaTime);
                
        Vector3 lookPos = player.gameObject.transform.position - gameObject.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(-lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * atkSpeed);
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
                Destroy(this.gameObject, 0.15f);
                break;
            case EnemyType.flyer:

                //DO ANIMATION AND PARTICLES
                Destroy(this.gameObject, 0.15f);
                break;
            case EnemyType.turret:

                oooCDReal = oooCD;
                outOfOrder = true;
                break;
        }
    }


    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "WPCollider")
        {
            Debug.Log(this.gameObject.name + "spotted player!");
            spotted = true;

            if (eType == EnemyType.walker)
            {
                if (outOfOrder == false)
                {
                    outOfOrder = true;
                    targetPos = player.transform.position + (player.transform.forward * 70f);
                    transform.rotation = Quaternion.FromToRotation(-transform.forward, (targetPos - gameObject.transform.position));
                }
            }
            else if (eType == EnemyType.flyer)
            {
                targetPos = player.gameObject.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.name == "WPCollider")
        {
            Debug.Log(this.gameObject.name + "lost player!");
            spotted = false;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.gameObject.name == "WPCollider")
        {
            if (eType == EnemyType.walker)
            {
                Debug.Log(this.gameObject.name + "collided with player!");
                coll.collider.GetComponentInParent<CartController>().GetStunned();
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawLine(gameObject.transform.position, targetPos);
    }
}
