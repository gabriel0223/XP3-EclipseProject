using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    private bool isClose;
    private PlayerInteraction playerInteraction;

    public GameObject textFilePrefab;
    public GameObject textFilesGroup;
    public GameObject terminalToBeInstantiated;
    private GameObject terminalGameObject;
    private GameObject canvas;
        
    // Start is called before the first frame update
    void Start()
    {
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (isClose)
        {
            if (Input.GetKeyDown(KeyCode.F) && !GameManager.instance.blockPlayerMovement)
            {
                OpenTerminal();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = true;
            playerInteraction.keyPressIcon.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = false;
            playerInteraction.keyPressIcon.SetActive(false);
        }
    }

    private void OpenTerminal()
    {
        terminalGameObject = Instantiate(terminalToBeInstantiated, canvas.transform);
        terminalGameObject.SetActive(true);
        GameManager.instance.blockPlayerMovement = true;
        AudioManager.instance.Play("TerminalAbrir");
    }
}
