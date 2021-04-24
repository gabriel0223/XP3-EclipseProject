using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZNavigation : MonoBehaviour
{
    private Door door;
    private bool isClose;
    private GameObject keyPressIcon;
    public ZDoor zDoor;

    // Start is called before the first frame update
    void Start()
    {
        door = GetComponent<Door>();
        keyPressIcon = GameObject.FindGameObjectWithTag("Player").transform.Find("KeyPressIcon").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClose && door.doorState == Door.DoorState.Open)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(LevelManager.instance.EnterDoor(zDoor));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = true;
            keyPressIcon.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = false;
            keyPressIcon.SetActive(false);
        }
    }
}
