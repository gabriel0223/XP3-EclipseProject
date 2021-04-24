using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PropulsorZeroG : MonoBehaviour
{
    public float jetpackForce;
    public float rotationForce;
    public float dashForce;
    private Rigidbody2D rb2d;
    private PlayerInteraction playerInteraction;
    public float deceleration;
    private AudioManager audioManager;
    private bool canPlaySound = true;

    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerInteraction = GetComponent<PlayerInteraction>();
        mainCam = Camera.main;
        audioManager = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.blockPlayerMovement)
        {
            var moveHreal = Input.GetAxisRaw("Horizontal");
            var moveAmountH = moveHreal * jetpackForce * Time.deltaTime;

            var moveV = Input.GetAxisRaw("Vertical");
            var moveAmountV = moveV * jetpackForce * Time.deltaTime;
        
            rb2d.AddForce(transform.right * moveAmountH);
            rb2d.AddForce(transform.up * moveAmountV);
            
            if (canPlaySound)
            {
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    audioManager.FadeInSound("JetpackSound", 1);
                    canPlaySound = false;
                }
            }

            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                if (audioManager.IfSoundIsPlaying("JetpackSound") && !canPlaySound)
                {
                    audioManager.FadeOutSound("JetpackSound", 0.75f);
                    canPlaySound = true;
                }
            }
            
            if (Input.GetKey(KeyCode.Q))
            {
                rb2d.AddTorque(rotationForce * Time.deltaTime);
            }
            else
            {
                if (rb2d.angularVelocity > 0)
                {
                    rb2d.AddTorque(-deceleration * Time.deltaTime);
                }
            }
        
            if (Input.GetKey(KeyCode.E))
            {
                rb2d.AddTorque(-rotationForce * Time.deltaTime);
            }
            else
            {
                if (rb2d.angularVelocity < 0)
                {
                    rb2d.AddTorque(deceleration * Time.deltaTime);
                }
            }
            
            //Debug.Log(rb2d.velocity.magnitude);
        }
        
        //rb2d.velocity += rb2d.velocity * -1 * 0.5f * Time.deltaTime;
        
        var dir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb2d.AddForce(dir * dashForce, ForceMode2D.Impulse);
        }
        
        //Debug.Log(rb2d.velocity.magnitude);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var collisionForce = other.relativeVelocity.magnitude;

        //Debug.Log(collisionForce);
        
        switch (collisionForce)
        {
            case float n when n >= 9:
                //Debug.Log("BATEU FORTE");
                break;

            case float n when n >= 5:
                //Debug.Log("BATEU MAIS OU MENOS");
                break;

            case float n when n >= 1:
                //Debug.Log("BATEU FRACO");
                break;
            default:
                //Debug.Log("FRAQUÍSSIMO");
                break;
        }
    }
}
