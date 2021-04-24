using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHorizontal : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb2d;
    private PlayerInteraction interactionScript;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        interactionScript = GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 moveAmount = new Vector2(moveInput * speed, 0f);

        if (!interactionScript.interacting)
        {
            rb2d.MovePosition(rb2d.position + moveAmount * Time.deltaTime);
        }

    }
}
