using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoEMapAttack : MonoBehaviour
{
    public float damage;
    public bool isAttacking;

    public float attackTime;
    private float attackTimeReal;

    public ParticleSystem ps;
    public GameObject damageArea;

    void Start()
    {
        isAttacking = false;
        damageArea.SetActive(false);
        ParticleSystem.EmissionModule em = ps.emission;
        em.enabled = false;
    }

    void Update()
    {
        ParticleSystem.EmissionModule em = ps.emission;

        if (isAttacking && attackTimeReal > 0)
        {
            attackTimeReal -= Time.deltaTime;
            if (em.enabled == false)
            {
                em.enabled = true;
            }
            if (damageArea.activeSelf == false)
            {                 
                damageArea.SetActive(true);
            }
        }
        else if (isAttacking)
        {
            isAttacking = false;
            em.enabled = false;
            damageArea.SetActive(false);
        }
    }

    public void Attack()
    {
        isAttacking = true;
        attackTimeReal = attackTime;
    }
}
