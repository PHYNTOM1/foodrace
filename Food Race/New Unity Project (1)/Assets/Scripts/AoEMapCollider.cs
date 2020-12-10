using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoEMapCollider : MonoBehaviour
{
    private float dmg;

    private void Start()
    {
        dmg = GetComponentInParent<AoEMapAttack>().damage;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "WPCollider")
        {
            Debug.Log(gameObject.name + " has damaged " + c.gameObject.name + " for " + dmg);

            //Do damage here, for example like this:
            //c.gameObject.GetComponentInParent<HealthControl>().Damage(dmg);
        }
    }
}
