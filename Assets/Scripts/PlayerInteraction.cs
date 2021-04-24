﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public bool canInteract;
    public GameObject keyPressIcon;
    [HideInInspector] public bool interacting;

    private Interactive interactiveObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.F) && !GameManager.instance.blockPlayerMovement)
            {
                interacting = true;
                interactiveObject.Interact();
                keyPressIcon.SetActive(false);
            }

            if (!interacting)
            {
                keyPressIcon.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactive"))
        {
            canInteract = true;
            interactiveObject = other.GetComponent<Interactive>();
            keyPressIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactive"))
        {
            canInteract = false;
            keyPressIcon.SetActive(false);
        }
    }
}