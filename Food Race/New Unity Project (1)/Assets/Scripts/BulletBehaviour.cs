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
    public int damage = 1;

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

    private void OnCollisionEnter(Collision coll)
    {
        if (this.gameObject.CompareTag("Bullet"))
        {
            if (coll.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("PLAYER HIT " + coll.collider.gameObject.name + " WITH A BULLET!");
                coll.collider.GetComponentInParent<EnemyBehaviour>().GetDamaged(damage);
            }
        }
        else if(this.gameObject.CompareTag("EnemyBullet"))
        {
            if (coll.collider.gameObject.name == "WPCollider")
            {
                Debug.Log("PLAYER GOT HIT BY BULLET!");
                coll.collider.GetComponentInParent<CartController>().GetStunned();
            }
        }
    }

}
