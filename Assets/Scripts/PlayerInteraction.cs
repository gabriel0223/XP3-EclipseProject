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
    public bool learningSomething;
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
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.F) && !GameManager.instance.blockPlayerMovement)
            {
                interacting = true;
                interactiveObject.Interact();
                keyPressIcon.SetActive(false);
            }

            if (!interacting && !learningSomething)
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
            objectBeingRepaired = other.gameObject;
        }
        else if (other.CompareTag("Searchable"))
        {
            canInteract = true;
            interactiveObject = null;
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
    }
}
