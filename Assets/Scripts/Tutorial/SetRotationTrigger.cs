using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotationTrigger : MonoBehaviour
{
    public bool setRotation;
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
        if (other.CompareTag("Player"))
        {
            if (setRotation == false)
            {
                other.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                other.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            }
            
            Destroy(gameObject);  
        }
        
    }
}
