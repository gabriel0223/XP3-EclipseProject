using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;

    public float rotationSmoothing;

    public float followSmoothing;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        ProCamera2D.Instance.AddCameraTarget(player);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Force16by9();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation,
            rotationSmoothing * Time.deltaTime);

        //transform.position = Vector2.Lerp(transform.position, player.position, followSmoothing * Time.deltaTime);
        
        // transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.position.x, followSmoothing * Time.deltaTime), 
        //     Mathf.Lerp(transform.position.y, player.position.y, followSmoothing * Time.deltaTime), -10);
    }
    
    private void Force16by9()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {  
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
        
            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
