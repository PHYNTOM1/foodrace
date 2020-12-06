using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMoverForTraining : MonoBehaviour
{

    public Vector3 addCoord;

    private void Start()
    {
        TrackMove[] objs = FindObjectsOfType<TrackMove>();
        List<GameObject> gos = new List<GameObject>();

        for (int i = 0; i < objs.Length; i++)
        {
            gos.Add(objs[i].gameObject);
        }

        foreach (GameObject item in gos)
        {
            item.transform.position += addCoord;
        }
    }
}
