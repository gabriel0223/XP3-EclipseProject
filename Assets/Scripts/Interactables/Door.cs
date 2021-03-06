using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public enum DoorState
    {
        Open, Closed, Opening, Closing
    }
    
    public Sprite lockedDoorSprite, unlockedDoorSprite;
    public Color lockedDoorLightColor, unlockedDoorLightColor;
    public LightSprite2D[] doorlights;
    //public LightSprite2D doorLight;
    public DoorState doorState;
    [HideInInspector] public string id; 
    public bool locked;
    public bool broken;
    public SpriteRenderer doorSr;
    private Animator anim;

    private void Awake()
    {
        id = SceneManager.GetActiveScene().name + gameObject.name + Math.Sqrt(transform.position.magnitude) + transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        doorState = DoorState.Closed;

        if (!CompareTag("VentDoor"))
            doorSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        
        doorSr.sprite = locked ? lockedDoorSprite : unlockedDoorSprite;
        
        if (!CompareTag("SideDoor")) return;

        foreach (var light in doorlights)
        {
            light.color = locked ? lockedDoorLightColor : unlockedDoorLightColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UnlockDoor()
    {
        locked = false;
        LevelManager.saveFile.Save(id, locked);
        LevelManager.saveFile.Sync();
        doorSr.sprite = unlockedDoorSprite;
        
        foreach (var light in doorlights)
        {
            light.color = unlockedDoorLightColor;
        }
    }
    
    public void LockDoor()
    {
        locked = true;
        LevelManager.saveFile.Save(id, locked);
        LevelManager.saveFile.Sync();
        StartCoroutine(LockWhenClosed());
    }

    public void OpenDoor()
    {
        if (locked)
            return;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity.magnitude == 0) //if player entered a door
        {
            anim.Play("DoorOpen", 0, 1f);
            doorState = DoorState.Open;
        }
        else
        {
            anim.SetTrigger("Open");
            
            if (gameObject.name.Equals("VentilationDoor"))
            {
                AudioManager.instance.Play("VentAbrir");
            }
            else
            {
                AudioManager.instance.Play("PortaLadoAbrir");    
            }
            
            StartCoroutine(ChangeDoorState(0, anim.GetCurrentAnimatorStateInfo(0).length));
        }
        
    }
    
    public void CloseDoor()
    {
        if (locked)
            return;
        
        anim.SetTrigger("Close");

        if (gameObject.name.Equals("VentilationDoor"))
        {
            AudioManager.instance.Play("VentFechar");
        }
        else
        {
            AudioManager.instance.Play("PortaLadoFechar");    
        }
        
        StartCoroutine(ChangeDoorState(1, anim.GetCurrentAnimatorStateInfo(0).length));
    }

    IEnumerator ChangeDoorState (int newState, float timeToChangeState)
    {
        doorState = newState == 0 ? DoorState.Opening : DoorState.Closing;

        yield return new WaitForSeconds(timeToChangeState);
        
        doorState = newState == 0 ? DoorState.Open : DoorState.Closed;
    }

    IEnumerator LockWhenClosed()
    {
        while (doorState != DoorState.Closed)
        {
            yield return null;
        }
        
        doorSr.sprite = lockedDoorSprite;

        foreach (var light in doorlights)
        {
            light.color = lockedDoorLightColor;
        }
        
        
        AudioManager.instance.Play("PortaLadoTravar");
    }
}
