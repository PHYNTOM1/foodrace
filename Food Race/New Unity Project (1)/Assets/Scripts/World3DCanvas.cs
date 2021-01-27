using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World3DCanvas : MonoBehaviour
{
    public bool rotating = false;

    public Transform camTransform;
    Quaternion originalRotation;

    void Start()
    {
        GameObject cam = GameObject.Find("UI Camera");

        GetComponent<Canvas>().worldCamera = cam.GetComponent<Camera>();
        camTransform = cam.transform;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (rotating)
        {
            transform.rotation = camTransform.rotation * originalRotation;
        }
    }
}
