using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDoor : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        anim.SetBool("Open", true);
        anim.SetBool("Closed", false);
        AudioManager.instance.Play("PortaLadoAbrir");
    }
    
    public void CloseDoor()
    {
        anim.SetBool("Open", false);
        anim.SetBool("Closed", true);
        AudioManager.instance.Play("PortaLadoFechar");
    }
}
