using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySensor : MonoBehaviour
{
    private Door door;

    // Start is called before the first frame update
    void Start()
    {
        door = transform.parent.GetComponent<Door>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //if (GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity == Vector2.zero) return;
            door.OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            door.CloseDoor();
        }
    }
}
