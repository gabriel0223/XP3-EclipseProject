using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    public Dialogue dialogue;
    public Image portrait;
    public AudioClip voice;
    private AudioSource audioSource;
    
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI speakerName;
    public String[] sentences;
    private int index;
    public float typeSpeed = 0.02f;
    public int lettersBetweenVoice;
    private Coroutine typing;
    private Coroutine speaking;
    private bool isSpeaking;
    public GameObject dialogueCompleteIcon;
    private PlayerInteraction playerInteractionScript;

    public GameObject[] speakerUIElements;
    
    public GameObject noSpeakerText, speakerText;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerInteractionScript = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerInteraction>();
        
        DialoguePlayer[] instances = FindObjectsOfType<DialoguePlayer>();

        if (instances.Length > 2) //if there's already an instance, delete the new one
        {
            Destroy(gameObject); 
        }
    }

    IEnumerator Type()
    {
        int count = 0;
        
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            count++;

            if (Char.IsLetterOrDigit(letter))
            {
                if (count >= lettersBetweenVoice)
                {
                    audioSource.Play();
                    count = 0;
                }
            }
            else
            {
                if (!letter.Equals(' ') && (!letter.Equals(',')))
                {
                    yield return new WaitForSeconds(typeSpeed * 10);
                }
                else if (letter.Equals(','))
                {
                    yield return new WaitForSeconds(typeSpeed * 5);
                }
            }
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        List<String>sentencesList = new List<string>();

        foreach (var sentence in dialogue.sentences)
        {
            if (sentence.speaker == null)
            {
                sentence.noSpeaker = true;
            }
            sentencesList.Add(sentence.text);
        }

        sentences = sentencesList.ToArray();
        
        CheckDialogueFormat();

        textDisplay.text = "";
        typing = StartCoroutine(Type());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            NextSentence();
        }
        
        if (textDisplay.text == sentences[index])
        {
            dialogueCompleteIcon.SetActive(true);
        }
        else
        {
            dialogueCompleteIcon.SetActive(false);
        }
    }

    public void NextSentence()
    {
        if (textDisplay.text == sentences[index]) //se o texto estiver 100% digitado na tela
        {
            if (index < sentences.Length - 1) //ir para a próxima linha de diálogo
            {
                index++;
                CheckDialogueFormat();
                textDisplay.text = "";
                typing = StartCoroutine(Type());
                
                //CheckDialogueFormat();
            }
            else
            {
                //textDisplay.text = "";
                
                Destroy(gameObject);
                playerInteractionScript.interacting = false;
            }
        }
        else
        {
            textDisplay.text = sentences[index];
            StopCoroutine(typing);
        }
    }

    public void CheckDialogueFormat()
    {
        if (dialogue.sentences[index].noSpeaker)
        {
            foreach (GameObject obj in speakerUIElements)
            {
                obj.SetActive(false); //disable all elements from speaker GUI, like portraits and char name boxes
            }

            textDisplay = noSpeakerText.GetComponent<TextMeshProUGUI>(); //text being autotyped switch to 'No Speaker Text'
            
            noSpeakerText.SetActive(true); //text formated to no speakers
        }
        else
        {
            foreach (GameObject obj in speakerUIElements)
            {
                obj.SetActive(true); //enable all elements from speaker GUI, like portraits and char name boxes
            }

            textDisplay = speakerText.GetComponent<TextMeshProUGUI>(); //text being autotyped switch to 'Speaker Text'
            
            noSpeakerText.SetActive(false); //text formated to no speakers
            
            //set names, portraits and voices
            speakerName.SetText(dialogue.sentences[index].speaker.name);
        
            portrait.sprite = dialogue.sentences[index].speaker.portrait;
            audioSource.clip = dialogue.sentences[index].speaker.voice;
        }
    }
}
