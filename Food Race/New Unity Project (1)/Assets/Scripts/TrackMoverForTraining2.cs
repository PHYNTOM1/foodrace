using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMoverForTraining2 : MonoBehaviour
{

    public Vector3 addCoord;

    private void Start()
    {
        TrackMove2[] objs = FindObjectsOfType<TrackMove2>();
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
