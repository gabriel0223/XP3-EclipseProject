using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationTrigger : MonoBehaviour
{
    public AudioConversation conversationToBeTriggered;
    public float delay;
    public bool playOnAwake;

    private void Awake()
    {
        if (!playOnAwake) return;
        StartConversation();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        StartConversation();
    }

    public void StartConversation()
    {
        ConversationManager.instance.StartConversation(conversationToBeTriggered, delay);
        Destroy(gameObject);
    }
}
