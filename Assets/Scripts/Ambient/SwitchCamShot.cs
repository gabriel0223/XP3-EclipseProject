using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamShot : MonoBehaviour
{
    private Camera camera;
    private float oldCamSize;
    public float newCamSize;
    public float smoothTime;
    private float diffBetweenSizes;
    private bool changingSize;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        oldCamSize = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    IEnumerator ChangeCameraSize(float newSize)
    {
        changingSize = true;
        float velocity = 0;
        while (Mathf.Abs(camera.orthographicSize - newSize) > 0.05f)
        {
            camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, newSize, ref velocity, smoothTime);
            yield return null;
        }

        changingSize = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (changingSize)
        {
            StopCoroutine("ChangeCameraSize");    
        }
        
        StartCoroutine("ChangeCameraSize", newCamSize);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (changingSize)
        {
            StopCoroutine("ChangeCameraSize");    
        }

        StartCoroutine("ChangeCameraSize", oldCamSize);
    }
}
