using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float maxDist = 10;
    public float lerpCounter = 0;

    void Update()
    {
        //lerpCounter += 0.001f;

        offset = target.forward * -15;    //camera rotates and pos according to cart vector3.forward
        offset.y = 12f;

        gameObject.transform.SetPositionAndRotation(target.position + offset, Quaternion.identity);

        /*
        if (Vector3.Distance(target.position, gameObject.transform.position) > maxDist)       //camera moves when it is out of the max range to target
        {
            lerpCounter += 0.001f;

            offset = target.forward * -2;    //camera rotates and pos according to cart vector3.forward
            offset.y = 3;

            Vector3 newpos = Vector3.Lerp(gameObject.transform.position, target.position + offset, lerpCounter);
            gameObject.transform.SetPositionAndRotation(newpos, Quaternion.identity);

        }
        else
        {
            lerpCounter = 0;
        }

        */
        gameObject.transform.LookAt(target);        //camera always keeps target in view
    }
}
