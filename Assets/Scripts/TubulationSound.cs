using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TubulationSound : MonoBehaviour
{
    public AudioMixerGroup audioMixerGroup;
    public AudioMixerSnapshot outsideTubulationSnapshot;
    public AudioMixerSnapshot onTubulationSnapshot;
    public float transitionTime;

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
        
        onTubulationSnapshot.TransitionTo(transitionTime);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        outsideTubulationSnapshot.TransitionTo(transitionTime);
    }
    
    
}
