using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    public Rigidbody2D rb2d;
    public Animator anim;
    public Collision coll;
    public SpriteRenderer sr;
    public Transform playerTransform;
    public PlayerMovement move;
    public GunScript gun;
    public int side = 1;
    public int lastWall;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collision>();
        sr = GetComponent<SpriteRenderer>();
        playerTransform = GetComponent<Transform>();
        //move = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (gun.angle < 90 && gun.angle > -90)
        {
            //sr.flipX = false;
            //playerTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            //sr.flipX = true;
            //playerTransform.localScale = new Vector3(-1, 1, 1);
        }
    }
}

// Update is called once per frame
//     void Update()
//     {
//         //WALKING
//         if (move.moveX > 0.01)
//         {
//             side = 1;
//             Flip(side);
//             if (coll.onGround)
//                 anim.SetBool("walking", true);
//         }
//         else if (move.moveX < -0.01)
//         {
//             side = -1;
//             Flip(side);
//             if (coll.onGround)
//                 anim.SetBool("walking", true);
//         }
//         else if (move.moveX == 0 && coll.onGround)
//         {
//             anim.SetBool("walking", false);
//         }
//
//         
//         //IDLE
//         if (!anim.GetBool("walking") && coll.onGround)
//         {
//             anim.SetBool("idle", true);
//         }
//         else
//         {
//             anim.SetBool("idle", false);
//         }
//         
//         //FALLING AND GROUND DETECTION
//         if (rb2d.velocity.y < 1 && !move.wallSlide)
//         {
//             anim.SetBool("falling", true);
//         }
//
//         if (rb2d.velocity.y > -1 && rb2d.velocity.y < 1)
//         {
//             anim.SetBool("falling", false);
//         }
//         else
//         {
//             anim.SetBool("idle", false);
//             anim.SetBool("walking", false);
//         }
//         
//         //WALL SLIDE
//         anim.SetBool("sliding", move.wallSlide);
//
//         if (coll.onLeftWall)
//         {
//             side = -1;
//             Flip(side);
//             lastWall = 1;
//         }
//
//         if (coll.onRightWall)
//         {
//             side = 1;
//             Flip(side);
//             lastWall = -1;
//         }
//
//         if (!move.canMove)
//         {
//             Flip(lastWall); 
//         }
//
//         if (move.dead)
//             sr.flipX = true;
//
//     }
//     
//     public void Flip(int side)
//     {
//         if (move.wallSlide)
//         {
//             if (side == -1 && sr.flipX)
//                 return;
//
//             if (side == 1 && !sr.flipX)
//             {
//                 return;
//             }
//         }
//
//         bool state = (side == 1) ? false : true;
//         sr.flipX = state;
//     }
// }
