using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeTutorial : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public AudioConversation conversationToBeTriggered;
    public float conversationDelay;
    public float impactForce;
    private bool hitHim;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //rb2d.AddForce(transform.right * impactForce, ForceMode2D.Impulse);
        rb2d.velocity = -transform.up * impactForce;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") || hitHim) return;

        hitHim = true;
        AudioManager.instance.Play("VisorRachadura02");
        GetComponentInChildren<TutorialTrigger>().ActivateTutorial();
        ConversationManager.instance.StartConversation(conversationToBeTriggered, conversationDelay);
    }
}
