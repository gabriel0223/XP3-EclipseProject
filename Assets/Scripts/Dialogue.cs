using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{

    [System.Serializable]
    public class Sentence
    {
        public Speaker speaker;
        [HideInInspector] public bool noSpeaker; 

        [TextArea(4, 20)] 
        public string text;
    }

    public Sentence[] sentences; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}



