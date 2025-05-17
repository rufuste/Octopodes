using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float jumpHeight;
    private float speed;
    private Rigidbody2D rb;
    private PlayerStats stats;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        speed = stats.Speed;
        jumpHeight = stats.JumpHeight;
    }
    
    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {

            rb.AddForce(transform.position * jumpHeight);
        }

        //Dodge???
        rb.linearVelocity = new Vector2(horizontalMovement, verticalMovement).normalized * speed;
    }
}
