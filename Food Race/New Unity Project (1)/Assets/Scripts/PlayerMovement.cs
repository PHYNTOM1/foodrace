using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Variables")]
    public float acceleration;
    public float maxSpeed;
    public float turnSpeed;

    [Space]
    public Vector2 movement = Vector2.zero;
    public Rigidbody cartRB;

    // Start is called before the first frame update
    void Start()
    {
        cartRB = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

    }

    private void FixedUpdate()
    {
        float moveSpeed = 0;
        float moveSpeedSide = 0;

        if (movement.y != 0)
        {
            moveSpeed = movement.y * acceleration * Time.fixedDeltaTime;
        }
        if (movement.x != 0)
        {
            moveSpeedSide = movement.x * turnSpeed * Time.fixedDeltaTime;
        }

        if (moveSpeed != 0)
        {
            cartRB.MovePosition(gameObject.transform.position + (Vector3.forward * moveSpeed) + (Vector3.right * moveSpeedSide));
        }

    }
}
