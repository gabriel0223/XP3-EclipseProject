using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public ParticleSystem[] psPropellant;

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
            PropellantParticles();
            
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

    private void PropellantParticles()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (move != Vector2.zero)
        {
            if (move.Equals(Vector2.right))
            {
                SetActiveParticles(new[] {1, 3});
            }
            else if (move.Equals(Vector2.left))
            {
                SetActiveParticles(new[] {2, 4});
            }
            else if (move.Equals(Vector2.up))
            {
                SetActiveParticles(new[] {3, 4});
            }
            else if (move.Equals(Vector2.down))
            {
                SetActiveParticles(new[] {1, 2});
            }
            else if (move.Equals(Vector2.one))
            {
                SetActiveParticles(new[] {3});
            }
            else if (move.Equals(-Vector2.one))
            {
                SetActiveParticles(new[] {2});
            }
            else if (move.Equals(new Vector2(-1, 1)))
            {
                SetActiveParticles(new[] {4});
            }
            else if (move.Equals(new Vector2(1, -1)))
            {
                SetActiveParticles(new[] {1});
            }
        }
        else
        {
            SetActiveParticles(new int[]{});
        }

        void SetActiveParticles(int[] remainingParticles)
        {
            foreach (var ps in psPropellant)
            {
                if (remainingParticles.Contains(Array.IndexOf(psPropellant, ps) + 1))
                {
                    PlayParticleSystem(Array.IndexOf(psPropellant, ps));
                }
                else
                {
                    StopParticleSystem(Array.IndexOf(psPropellant, ps));
                }
            }
        }

        void PlayParticleSystem(int index)
        {
            if (!psPropellant[index].isPlaying) psPropellant[index].Play();
        }
        
        void StopParticleSystem(int index)
        {
            if (psPropellant[index].isPlaying) psPropellant[index].Stop();
        }
    }
}
