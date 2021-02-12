using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawning : MonoBehaviour
{
    public GameObject eWalker;
    public GameObject eFlyer;
    public GameObject eTurret;

    public List<Transform> walkerTs;
    public List<Transform> flyerTs;


    public void SpawnNewEnemies()
    {
        foreach (Transform tf in walkerTs)
        {
            Instantiate(eWalker, tf.position, Quaternion.identity, null);
        }
        foreach (Transform tf in flyerTs)
        {
            Instantiate(eFlyer, tf.position, Quaternion.identity, null);
        }
    }

    private void KillAllEnemies()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<EnemyBehaviour>().eType != EnemyBehaviour.EnemyType.turret)
            {
                Destroy(go);
            }
        }
    }

    public void RespawnEnemies()
    {
        KillAllEnemies();
        SpawnNewEnemies();
    }
}
