﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
    public float sSpeed = 8.5f;
    public Vector3 dist;
    public Transform lookTarget;

    void FixedUpdate()
    {
        Vector3 dpos = cameraTarget.position + dist;
        Vector3 sPos = Vector3.Lerp(transform.position, dpos, sSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.position);
    }
}

