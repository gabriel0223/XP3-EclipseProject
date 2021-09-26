using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propulsor : MonoBehaviour
{
    private Collision coll;
    private Rigidbody2D rb2d;
    public float jetpackMaxSpeed;
    public float jetpackAcceleration;
    private float jetpackSpeed;
    private bool gravityInverted;
    
    
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Space) && !coll.onGround)
        {
            if (jetpackSpeed < jetpackMaxSpeed)
                jetpackSpeed += jetpackAcceleration * Time.deltaTime;

            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y + jetpackSpeed);
            
            if (rb2d.velocity.y > jetpackMaxSpeed)
                rb2d.velocity = new Vector2(rb2d.velocity.x, jetpackMaxSpeed);
            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jetpackSpeed = 0;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Physics2D.gravity = -Physics2D.gravity;
            // if (gravityInverted)
            // {
            //     gravityInverted = false;
            //     rb2d.gravityScale = 2;
            // }
            // else
            // {
            //     gravityInverted = true;
            //     rb2d.gravityScale = -5;
            // }
        }
    }
    
}
