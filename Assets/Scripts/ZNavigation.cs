using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZNavigation : MonoBehaviour
{
    [System.Serializable]
    public class ZDoor
    {
        public string doorID;
        public string nextScene;
        public string nextDoorID;
    }
    
    private PlayerInteraction playerInteraction;
    private Door door;
    private bool isClose;
    private GameObject keyPressIcon;
    public ZDoor zDoor;
    
    
    
    // public string doorID;
    // public string nextScene;
    // public string nextDoorID;

    private void Awake()
    {
        door = GetComponent<Door>();
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        keyPressIcon = playerInteraction.keyPressIcon;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isClose && !GameManager.instance.interactingUI && door.doorState == Door.DoorState.Open)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(LevelManager.instance.EnterDoor(zDoor));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !door.locked)
        {
            isClose = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !door.locked)
        {
            isClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = false;
        }
    }
}
