using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Audio Conversation")]
public class AudioConversation : ScriptableObject
{
    [System.Serializable]
    public class AudioSentence
    {
        public AudioClip audioClip;
        public float timeToPlayNext;
    }

    public AudioSentence[] audioSentences;
}
