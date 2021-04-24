using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorsTrigger : MonoBehaviour
{
    public Door[] doorsToBeLocked;
    public string soundWarningName;
    public float timeToPlayWarning = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        foreach (var door in doorsToBeLocked)
        {
            if (door.doorState == Door.DoorState.Open || door.doorState == Door.DoorState.Opening)
            {
                door.CloseDoor();
            }
            door.LockDoor();
        }
        
        Invoke(nameof(PlayWarning),timeToPlayWarning);
        Destroy(gameObject, timeToPlayWarning);
    }

    private void PlayWarning()
    {
        AudioManager.instance.Play(soundWarningName);
    }
}
