using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilationTrigger : MonoBehaviour
{
    private Animator[] ventilationShafts;
    public Transform ventilationEntry;
    public LayerMask solidLayers;

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

        Collider2D[] walls = Physics2D.OverlapBoxAll(ventilationEntry.position, new Vector2(2, 2), 0, solidLayers);
        
        foreach (var wall in walls)
        {
            wall.isTrigger = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (var shaft in ventilationShafts)
        {
            shaft.SetTrigger("FadeOut");
        }
        
        Collider2D[] walls = Physics2D.OverlapBoxAll(ventilationEntry.position, new Vector2(2, 2), 0, solidLayers);
        
        foreach (var wall in walls)
        {
            wall.isTrigger = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(ventilationEntry.position, new Vector2(2, 2));
    }
}
