using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEngine : MonoBehaviour
{
    private Repairable repairScript;
    private Rigidbody2D playerRb;
    private Animator engineAnimator;
    private bool exploded;
    public Vector2 explosionForce;
    public GameObject blackScreen;
    private Transform canvas;
    public float blackScreenDuration;
    private void Awake()
    {
        repairScript = GetComponent<Repairable>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        engineAnimator = transform.parent.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (repairScript.repairedPorcentage > 0.6f && !exploded)
        {
            repairScript.repaired = true;
            exploded = true;
            engineAnimator.SetTrigger("Explode");
            playerRb.angularDrag = 1;
            playerRb.AddForce(explosionForce * 7, ForceMode2D.Impulse); //throw the player away
            
            playerRb.AddForceAtPosition(explosionForce, new Vector2(playerRb.position.x, playerRb.position.y + 
                Vector2.Dot(playerRb.transform.up, Vector2.down) * -1), ForceMode2D.Impulse);
            //spin the player so he hits his head

            AudioManager.instance.Play("Explosion");
            StartCoroutine("TurnToBlack");
        }
    }

    IEnumerator TurnToBlack()
    {
        yield return new WaitForSeconds(0.7f);
        Instantiate(blackScreen, canvas);
        AudioManager.instance.Stop(AudioManager.instance.musicPlaying);
        AudioListener.pause = true;
        
        yield return new WaitForSeconds(blackScreenDuration);
        StartCoroutine(LevelManager.instance.GoToScene(5));
    }
}
