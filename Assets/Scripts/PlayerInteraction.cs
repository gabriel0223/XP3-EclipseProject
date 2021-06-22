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
    public Repairable currentRepairable;
    public bool learningSomething;
    private Interactive interactiveObject;
    private PropulsorZeroG propulsor;

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

    private void Awake()
    {
        propulsor = GetComponent<PropulsorZeroG>();
    }

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
                if (interactiveObject != null) interactiveObject.Interact();
                propulsor.Decelerate();
                keyPressIcon.SetActive(false);
            }

            if (!interacting && !learningSomething)
            {
                keyPressIcon.SetActive(true);
            }
        }

        if (canRepair && !currentRepairable.repaired)
        {
            if (Input.GetKey(KeyCode.F))
            {
                GameManager.instance.blockPlayerMovement = true;
                propulsor.Decelerate();
                repairBar.FillBar();
            }
            else
            {
                GameManager.instance.blockPlayerMovement = false;
            }
            
            if (currentRepairable.repairedPorcentage >= 1)
            {
                currentRepairable.CompleteRepair();
                keyHoldIcon.SetActive(false);
            }
        }
    }
    
    public IEnumerator LearnSomething()
    {
        yield return null;
        // learningSomething = true;
        // yield return new WaitForSeconds(6);
        // learningSomething = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
            currentRepairable = other.GetComponent<Repairable>();
            repairBar.currentRepairable = currentRepairable;
        }
        else if (other.CompareTag("Searchable"))
        {
            canInteract = true;
            interactiveObject = null;
            keyPressIcon.SetActive(true);
        }
        else if (other.CompareTag("ZDoor"))
        {
            if (other.GetComponent<Door>().locked) return;
            
            canInteract = true;
            keyPressIcon.SetActive(true);
        }
        
        if (learningSomething) keyPressIcon.SetActive(false);
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
        else if (other.CompareTag("Searchable"))
        {
            canInteract = false;
            keyPressIcon.SetActive(false);
        }
        else if (other.CompareTag("ZDoor"))
        {
            if (other.GetComponent<Door>().locked) return;
            
            canInteract = false;
            keyPressIcon.SetActive(false);
        }
    }
}
