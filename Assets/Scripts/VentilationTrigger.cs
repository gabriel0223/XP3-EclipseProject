using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilationTrigger : MonoBehaviour
{
    private Animator[] ventilationShafts;

    private void Awake()
    {
        ventilationShafts = GetComponentsInChildren<Animator>(true);
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

        foreach (var shaft in ventilationShafts)
        {
            shaft.SetTrigger("FadeIn");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (var shaft in ventilationShafts)
        {
            shaft.SetTrigger("FadeOut");
        }
    }
}
