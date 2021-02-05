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

    public GameObject hitImpact;
    public GameObject hitImpact2;

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
        if (coll.collider.gameObject.name == "WPCollider")
        {            
            if (this.gameObject.CompareTag("EnemyBullet"))
            {
                GameObject h = Instantiate(hitImpact2, coll.GetContact(0).point, Quaternion.identity);
                Destroy(h, 2f);

                coll.collider.GetComponentInParent<CartController>().GetStunned();

                Destroy(gameObject);
            }
        }
        else
        {
            if (this.gameObject.CompareTag("Bullet"))
            {
                GameObject h = Instantiate(hitImpact, coll.GetContact(0).point + new Vector3(0f, 0.4f, 0f), Quaternion.identity);
                Destroy(h, 2f);

                if (coll.collider.gameObject.CompareTag("Enemy"))
                {
                    coll.collider.GetComponentInParent<EnemyBehaviour>().GetDamaged(damage);
                }
            }
            else if (this.gameObject.CompareTag("EnemyBullet"))
            {
                GameObject h = Instantiate(hitImpact2, coll.GetContact(0).point + new Vector3(0f, 0.8f, 0f), Quaternion.identity);
                Destroy(h, 2f);

                Destroy(gameObject);
            }

            Destroy(gameObject);
        }
    }

}
