using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorState
    {
        Open, Closed, Opening, Closing
    }
    
    public Sprite lockedDoorSprite, unlockedDoorSprite;
    public DoorState doorState;
    public bool locked;
    private SpriteRenderer doorSr;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        doorState = DoorState.Closed;

        if (!CompareTag("SideDoor")) return;
        
        doorSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        doorSr.sprite = locked ? lockedDoorSprite : unlockedDoorSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UnlockDoor()
    {
        locked = false;
        doorSr.sprite = unlockedDoorSprite;
    }
    
    public void LockDoor()
    {
        locked = true;
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
            AudioManager.instance.Play("PortaLadoAbrir");
            StartCoroutine(ChangeDoorState(0, anim.GetCurrentAnimatorStateInfo(0).length));
        }
        
    }
    
    public void CloseDoor()
    {
        if (locked)
            return;
        
        anim.SetTrigger("Close");
        AudioManager.instance.Play("PortaLadoFechar");
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
        AudioManager.instance.Play("PortaLadoTravar");
    }
}
