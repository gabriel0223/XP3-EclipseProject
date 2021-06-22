using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Repairable : MonoBehaviour
{
    private Transform player;
    public Transform repairPoint;
    public float repairSpeed;
    private GameObject repairButton;
    [Range(0, 1)] 
    public float repairedPorcentage;

    private SpriteRenderer sr;
    public float fadeSpeed;
    public bool repaired;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        repairButton = transform.GetChild(0).gameObject;
        sr = repairButton.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!repaired)
        {
            repairButton.transform.rotation = player.rotation;
            
            var fade = Mathf.SmoothStep(0, 1, Mathf.PingPong(fadeSpeed * Time.time, 1));
            sr.color = new Color(1, 1, 1, fade);
        }
    }

    public void CompleteRepair()
    {
        tag = "Untagged";
        repaired = true;
        Destroy(repairButton);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (repaired) return;
        
        repairButton.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (repaired) return;
        
        repairButton.SetActive(true);
    }
}
