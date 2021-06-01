using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public bool canInteract;
    public bool canRepair;
    public GameObject keyPressIcon;
    public GameObject keyHoldIcon;
    [HideInInspector] public bool interacting;
    public ProgressBar repairBar;
    public GameObject objectBeingRepaired;
    private bool learningSomething;
    private Interactive interactiveObject;

    public bool LearningSomething
    {
        get => learningSomething;
        
        set
        {
            learningSomething = value;
            if (value)
            {
                StartCoroutine(LearnSomething());
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (learningSomething)
        {
            keyPressIcon.SetActive(false);
        }

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

        if (canRepair)
        {
            if (Input.GetKey(KeyCode.F))
            {
                repairBar.FillBar();
            }
            
            if (repairBar.barImage.fillAmount >= 1)
            {
                objectBeingRepaired.tag = "Untagged";
                keyHoldIcon.SetActive(false);
            }
        }
    }
    
    public IEnumerator LearnSomething()
    {
        learningSomething = true;
        yield return new WaitForSeconds(5);
        learningSomething = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (learningSomething) return;
        
        if (other.CompareTag("Interactive"))
        {
            canInteract = true;
            interactiveObject = other.GetComponent<Interactive>();
            keyPressIcon.SetActive(true);
        }
        else if (other.CompareTag("Repairable"))
        {
            canRepair = true;
            keyHoldIcon.SetActive(true);
            objectBeingRepaired = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactive"))
        {
            canInteract = false;
            keyPressIcon.SetActive(false);
        }
        else if (other.CompareTag("Repairable"))
        {
            canRepair = false;
            keyHoldIcon.SetActive(false);
        }
    }
}
