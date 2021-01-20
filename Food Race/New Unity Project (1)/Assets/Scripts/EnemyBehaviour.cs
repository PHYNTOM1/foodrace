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

    public float atkSpeed = 1f;
    [SerializeField]
    private float realAtkSpeed = 0f;
    public GameObject enemyBullet;
    public float left = 2f;

    void Update()
    {
        switch (eType)
        {
            case EnemyType.walker:

                if (spotted)
                {
                    MoveInWay();
                }
                else
                {
                    RandomWalking();
                }
                break;
            case EnemyType.flyer:
                
                if (spotted)
                {
                    MoveToPlayer();
                }
                else
                {
                    RandomWalking();
                }
                break;
            case EnemyType.turret:

                if (spotted)
                {
                    DoShooting();
                }

                if (realAtkSpeed > 0)
                {
                    realAtkSpeed -= Time.deltaTime;
                }
                break;
        }
    }

    public void MoveInWay()
    {
        //calculate player position + forward by certain distance, save this position and move there, then turn to face player        
    }

    public void RandomWalking()
    {
        //wander around on track and check for player
    }

    public void MoveToPlayer()
    {
        //fly straight to the player,
        //if in range, start circling around him
        //decrease player topSpeed and acceleration
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

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "WPCollider")
        {
            Debug.Log(this.gameObject.name + "spotted player!");
            spotted = true;
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
            Debug.Log(this.gameObject.name + "collided with player!");
            coll.collider.GetComponentInParent<CartController>().GetStunned();
        }
    }


}
