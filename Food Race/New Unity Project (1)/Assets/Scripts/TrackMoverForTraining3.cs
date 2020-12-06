using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMoverForTraining3 : MonoBehaviour
{

    public Vector3 addCoord;

    private void Start()
    {
        TrackMove3[] objs = FindObjectsOfType<TrackMove3>();
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
