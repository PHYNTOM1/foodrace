using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoEMapCollider : MonoBehaviour
{
    private int dmg;

    private void Start()
    {
        dmg = GetComponentInParent<AoEMapAttack>().damage;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "WPCollider")
        {
            Debug.Log(gameObject.name + " has damaged " + c.gameObject.name + " for " + dmg);

            if (c.gameObject.GetComponentInParent<HealthControl>() != null)
            {
                c.gameObject.GetComponentInParent<HealthControl>().Damage(dmg);
            }
        }
    }
}
