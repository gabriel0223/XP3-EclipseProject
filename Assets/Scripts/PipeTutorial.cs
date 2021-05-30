using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeTutorial : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float impactForce;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //rb2d.AddForce(transform.right * impactForce, ForceMode2D.Impulse);
        rb2d.velocity = -transform.up * impactForce;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
