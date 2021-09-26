using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb2d;
    private Collision coll;
    private PlayerAnimation anim;
    public LevelLoader levelLoader;
    public float x, y, xRaw, yRaw;
    private Vector2 dir;
    public ParticleSystem footDust;

    public ParticleSystem slideDust;
    private ParticleSystem.EmissionModule slideEmission;

    [Header("STATS")]
    public float speed = 10f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float slideSpeed;
    public float wallJumpLerp = 10;
    public float wallJumpKnockback = 400;
    public float coyoteStartTime;
    private float coyoteTime;
    public float jumpDelay = 0.2f;
    private float jumpTimer;
    public int side;
    public float timeToDie;

    [Header("BOOLEANS")]
    public bool wallGrab;

    public bool wallSlide;
    public bool wallJumped;
    public bool canMove = true;
    public bool jumped;
    public bool dead;
    public bool hasSlideSoundPlayed;

    [Header("CAMERA CONTROLS")] 
    public Transform camTarget;
    public Collider2D camDeadZone;

    public float aheadAmount, aheadSpeed;

    public bool cameraMoving;

    public bool stopX;



    [HideInInspector] public float moveX = 0;

    public float acceleration;
    public float deceleration;
    
    public float accelerationOnAir;
    public float decelerationOnAir;

    private float originalAcceleration, originalDeceleration;


    // Start is called before the first frame update
    void Start()
    {
        originalAcceleration = acceleration;
        originalDeceleration = deceleration;
        
        rb2d = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        anim = GetComponent<PlayerAnimation>();
        slideEmission = slideDust.emission;
    }

    // Update is called once per frame
    void Update()
    {

        //MOVEMENT
        moveX = Mathf.Clamp(moveX, -1, 1);

        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) //if it's not pressing anything
        {
            if (moveX < 0.2f && moveX > -0.2f) //if it's almost zero, don't move
            {
                moveX = 0;
            }
        }

        if (Input.GetKey(KeyCode.D)) //going to the right
        {
            moveX += acceleration * Time.deltaTime; //accelerate
        }
        else
        {
            if (moveX > 0) //if it stops
            {
                moveX -= deceleration * Time.deltaTime; //decelerate
            }
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            moveX -= acceleration * Time.deltaTime;
        }
        else
        {
            if (moveX < 0)
            {
                moveX += deceleration * Time.deltaTime;
            }
        }

        if (coll.onGround)
        {
            acceleration = originalAcceleration;
            deceleration = originalDeceleration;
            
            dir = new Vector2(moveX, y);
        }
        else //if it's on air
        {
            acceleration = accelerationOnAir;
            deceleration = decelerationOnAir;
            
            dir = new Vector2(moveX, y);
        }

        //GRAB THE WALL
        //wallGrab = coll.onWall && Input.GetKey(KeyCode.LeftShift);
        
        if (wallGrab && canMove)
        {
            wallSlide = false;
            rb2d.velocity = new Vector2(rb2d.velocity.x, y * speed);
        }

        //WALL SLIDE
        if (!coll.onGround && !wallGrab)
        {
            if (coll.onWall)
            {
                wallJumped = true;

                if (rb2d.velocity.y < 1)
                { 
                    WallSlide();
                    AudioManager.instance.Play("Sliding");
                }
            }
        }

        if (!coll.onWall && !coll.onGround)
            wallSlide = false;
        
        if (coll.onGround)
            wallSlide = false;
        
        if (!wallSlide)
            AudioManager.instance.Stop("Sliding");
            

        if (Input.GetKeyUp(KeyCode.LeftShift) || coll.onGround || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (coll.onGround && coll.onWall)
        {
            wallSlide = false;
        }

        //JUMP
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            jumpTimer = Time.time + jumpDelay;
            
        }

        if (coll.onWall && !coll.onGround && jumpTimer > Time.time)
        {
            //footDust.transform.position = new Vector3(gameObject.transform.position.x + side * 0.4f, gameObject.transform.position.y, gameObject.transform.position.z);
            //footDust.transform.rotation = Quaternion.Euler(0, 0, 90 * side);
            WallJump();
        }

        
        if (coyoteTime > 0f && !jumped)
        {
            if (jumpTimer > Time.time)
            {
                coyoteTime = -1;
                jumped = true;
                Invoke("UnlockJump", 0.5f);
                //transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.8f, gameObject.transform.position.z);
                Jump(Vector2.up);
            }
        }
        
        if (coll.onGround)
        {
            wallJumped = false;
            coyoteTime = coyoteStartTime;
            //footDust.transform.rotation = Quaternion.Euler(0, 0, 0);
            
        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }
        
        //PLAYER FOOTSTEPS
        side = coll.onRightWall ? 1 : -1;

        if (wallSlide)
        {
            slideEmission.rateOverTime = 20;
        }
        else
        {
            slideEmission.rateOverTime = 0;
        }

    }

    private void FixedUpdate()
    {
        //CONTROLAR O PERSONAGEM
        Walk(dir);
        
        //SE O JOGADOR ESTIVER NO CHÃO


        //SE O JOGADOR PULAR 
     

                
        //if (coll.onWall && !coll.onGround)


        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb2d.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Joystick1Button1))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; 
        }
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;
        
        if (!wallJumped)
            rb2d.velocity = new Vector2(dir.x * speed, rb2d.velocity.y);
        if (wallJumped)
            rb2d.velocity = Vector2.Lerp(rb2d.velocity, new Vector2(dir.x * speed, rb2d.velocity.y),
                wallJumpLerp * Time.deltaTime);
        
    }

    private void Jump(Vector2 dir)
    {
        //footDust.Play();
        AudioManager.instance.Play("Jump");
        anim.anim.SetTrigger("jump");
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.velocity += dir * jumpForce;
        jumpTimer = 0;
    }

    private void WallSlide()
    {
        if (!canMove)
            return;
        
        wallSlide = true;
        rb2d.velocity = new Vector2(rb2d.velocity.x, -slideSpeed);
        slideDust.transform.position = new Vector3(gameObject.transform.position.x + side * 0.4f, gameObject.transform.position.y, gameObject.transform.position.z);
        slideDust.transform.rotation = Quaternion.Euler(0, 0, 90 * side);

    }
    
    private void WallJump()
    {
        wallJumped = true;

        StartCoroutine(DisableMovement(.2f));
        StopCoroutine(DisableMovement(0));
        
        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;
        
        Jump((Vector2.up / 1.2f));
        
        rb2d.AddForce(new Vector2(wallJumpKnockback * wallDir.x, 0));
    }

    private void UnlockJump()
    {
        jumped = false;
    }
    
    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void ResetLevel()
    {
        StartCoroutine(levelLoader.ResetLevel());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
        {
            AudioManager.instance.Play("DeathSound");
            dead = true;
            rb2d.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            anim.anim.SetTrigger("death");
            Invoke("ResetLevel", timeToDie);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            AudioManager.instance.Play("DeathSound");
            dead = true;
            rb2d.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            anim.anim.SetTrigger("death");
            Invoke("ResetLevel", timeToDie);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("Cam DeadZone"))
        {
            cameraMoving = true;
        }
    }
}
