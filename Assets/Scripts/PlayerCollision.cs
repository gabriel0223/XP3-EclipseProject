using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
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
        PlayCollisionSounds(collisionForce);
    }

    void PlayCollisionSounds(float collisionForce)
    {
        var colVol = -5 + collisionForce;
        
        collisionMixer.SetFloat("ColVol", Mathf.Clamp(colVol, -5, 5));
        collisionMixer.SetFloat("ColPitch",  Random.Range(collisionForce/14, collisionForce/10));
        collisionMixer.SetFloat("ColReverb_Reverb", collisionForce * 100);
        AudioManager.instance.PlayRandomBetweenSounds(new []{"WeakImpact1", "WeakImpact2", "WeakImpact3"});

        if (collisionForce > 8)
        {
            AudioManager.instance.PlayRandomBetweenSounds(new []{"VisorRachadura01", "VisorRachadura02, VisorRachadura03", "VisorRachadura04"});
            //ProCamera2DShake.Instance.Shake("PistolCamShake");
            GameManager.playerHealth -= Random.Range(collisionForce / 2, collisionForce);
        }

        StartCoroutine("LockCollisionSound");
    }
    
    IEnumerator LockCollisionSound()
    {
        canPlayColSound = false;
        yield return new WaitForSeconds(0.25f);
        canPlayColSound = true;
    }
}
