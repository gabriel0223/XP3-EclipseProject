using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCollision : MonoBehaviour
{
    private bool canPlayColSound = true;
    public AudioMixer collisionMixer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!canPlayColSound) return;

        var collisionForce = other.relativeVelocity.magnitude;

        Debug.Log(collisionForce);
        PlayCollisionSound(collisionForce);
    }

    void PlayCollisionSound(float collisionForce)
    {
        var colVol = -5 + collisionForce;
        
        collisionMixer.SetFloat("ColVol", Mathf.Clamp(colVol, -5, 5));
        collisionMixer.SetFloat("ColPitch",  Random.Range(collisionForce/14, collisionForce/10));
        collisionMixer.SetFloat("ColReverb_Reverb", collisionForce * 100);
        //collisionMixer.SetFloat("ColLowpass_Freq", collisionForce * 500);
        // AudioManager.instance.Play("WeakImpact1");
        AudioManager.instance.PlayRandomBetweenSounds(new []{"WeakImpact1", "WeakImpact2", "WeakImpact3"});
        
        
        // switch (collisionForce)
        // {
        //     case float n when n >= 9:
        //         Debug.Log("BATEU FORTE");
        //         //AudioManager.instance.PlayRandomBetweenSounds(new []{"StrongImpact1", "StrongImpact2", "StrongImpact3"});
        //         //AudioManager.instance.PlayRandomBetweenSounds(new []{"WeakImpact1", "WeakImpact2", "WeakImpact3"});
        //         collisionMixer.SetFloat("ColPitch", 15);
        //         AudioManager.instance.Play("WeakImpact1");
        //         break;
        //
        //     case float n when n >= 5:
        //         Debug.Log("BATEU MAIS OU MENOS");
        //         //AudioManager.instance.PlayRandomBetweenSounds(new []{"MediumImpact1", "MediumImpact2", "MediumImpact3"});
        //         //AudioManager.instance.PlayRandomBetweenSounds(new []{"WeakImpact1", "WeakImpact2", "WeakImpact3"});
        //         AudioManager.instance.Play("WeakImpact1");
        //         break;
        //
        //     case float n when n >= 1:
        //         Debug.Log("BATEU FRACO");
        //         //AudioManager.instance.PlayRandomBetweenSounds(new []{"WeakImpact1", "WeakImpact2", "WeakImpact3"});
        //         
        //         collisionMixer.SetFloat("ColPitch",  Random.Range(collisionForce/12, collisionForce/8));
        //         AudioManager.instance.Play("WeakImpact1");
        //         break;
        //     default:
        //         //Debug.Log("FRAQUÍSSIMO");
        //         break;
        // }

        StartCoroutine("LockCollisionSound");
    }
    
    IEnumerator LockCollisionSound()
    {
        canPlayColSound = false;
        yield return new WaitForSeconds(0.25f);
        canPlayColSound = true;
    }
}
