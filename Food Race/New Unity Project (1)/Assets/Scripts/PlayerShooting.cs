using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;
    private bool shooting = false;
    public float shootCD = 0f;
    [SerializeField]
    private float shootCDReal = 0f;
    public float distToKart = 2f;
    private float left = 1f;
    private CartController cc;

    void Start()
    {
        shooting = false;
        cc = GetComponent<CartController>();
    }

    void Update()
    {
        if (cc.notRacing == false && cc.stunned == false)
        {
            if (shooting == true && shootCDReal <= 0)
            {
                Shoot();            
            }
        }

        if (shootCDReal > 0)
    	{
            shootCDReal -= Time.deltaTime;
    	}

        shooting = false;
    }

    public void ShootInput(bool b)
    {
        shooting = b;
    }

    private void Shoot()
    {
        GameObject b = Instantiate(bullet, (this.gameObject.transform.position + (this.gameObject.transform.forward.normalized * distToKart) + (this.gameObject.transform.right.normalized * left * 0.5f)), Quaternion.Euler(90f + this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, this.gameObject.transform.eulerAngles.z));
        
        if (left == 1)
        {
            left = -1f;
        }
        else
        {
            left = 1f;
        }

        b.transform.parent = null;

        shootCDReal = shootCD;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine((this.gameObject.transform.position + (this.gameObject.transform.forward.normalized * distToKart)), (this.gameObject.transform.position + (this.gameObject.transform.forward.normalized * distToKart * 2)));
    }
}
