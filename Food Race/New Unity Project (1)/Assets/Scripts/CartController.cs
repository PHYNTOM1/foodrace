using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{

    public WheelCollider[] wheelColls;
    public GameObject[] wheelMeshs;
    
    [Space]
    public Transform massCenter;

    public float maxMotorTorque;
    public float maxSteering;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = massCenter.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteering * Input.GetAxis("Horizontal");

        wheelColls[0].steerAngle = steering;
        wheelColls[1].steerAngle = steering;

        foreach (WheelCollider wc in wheelColls)
        {
            wc.motorTorque = motor;

        }

        CollToMeshRotation();
    }



    public void CollToMeshRotation()
    {
        Quaternion rot;
        Vector3 pos;

        for (int i = 0; i < wheelColls.Length; i++)
        {
            wheelColls[i].GetWorldPose( out pos, out rot);
            wheelMeshs[i].transform.position = pos;
            wheelMeshs[i].transform.rotation = rot;
            wheelMeshs[i].transform.eulerAngles = new Vector3(0, rot.eulerAngles.y, 90);
        }
    }
}
