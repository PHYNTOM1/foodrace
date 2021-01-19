using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BulletBehaviour : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 3f;
    public float deathRange = 2f;
    private Vector3 dir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, deathRange);
        dir = this.gameObject.transform.up;
    }

    void FixedUpdate()
    {
        rb.AddForce(dir * moveSpeed * 1000f * Time.deltaTime, ForceMode.Impulse);
    }

}
