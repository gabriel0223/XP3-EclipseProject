using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]
    
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;

    [Space] 
    
    [Header("Collision")]
    public float collisionRadius = 0.25f;
    public float wallCollisionRadius = 0.15f;

    public Vector2 bottomOffset, leftOffset, rightOffset;
    
    private Color debugCollisionColor = Color.red;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle((Vector2) transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2) transform.position + leftOffset, wallCollisionRadius, groundLayer) ||
            Physics2D.OverlapCircle((Vector2) transform.position + rightOffset, collisionRadius, groundLayer);
        
        onLeftWall = Physics2D.OverlapCircle((Vector2) transform.position + leftOffset, wallCollisionRadius, groundLayer);
        onRightWall =  Physics2D.OverlapCircle((Vector2) transform.position + rightOffset, wallCollisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] {bottomOffset, rightOffset, leftOffset};
        
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, wallCollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, wallCollisionRadius);

    }
}
