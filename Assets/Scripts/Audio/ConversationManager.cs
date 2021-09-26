using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioConversation currentConversation;
    public static ConversationManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator PlayConversation(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        for (int i = 0; i < currentConversation.audioSentences.Length; i++)
        {
            audioSource.clip = currentConversation.audioSentences[i].audioClip;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length + currentConversation.audioSentences[i].timeToPlayNext);
        }
    }

    public void StartConversation(AudioConversation conversation, float delay)
    {
        currentConversation = conversation;
        StartCoroutine("PlayConversation", delay);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
