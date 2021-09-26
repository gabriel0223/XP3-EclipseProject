using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateAngle : MonoBehaviour
{
    private Transform player; 
        
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawAngle();

        if (Vector2.Distance(transform.position, player.position) > 1f)
        {
            transform.Translate(-transform.right * 0.01f, Space.World);
        }

        bool isLowEnough = transform.position.y < 16;
    }

    private void DrawAngle()
    {
        Vector2 enemyForward = -transform.right;
        Vector2 playerDirection = player.position - transform.position;
        
        Debug.DrawRay(transform.position, enemyForward * 10, Color.green);
        Debug.DrawRay(transform.position, playerDirection, Color.red);
        var angle = Vector3.SignedAngle(enemyForward, playerDirection, transform.forward);
        // Debug.Log("Angle: " + Mathf.Acos(Vector2.Dot(enemyForward, playerDirection) / enemyForward.magnitude * playerDirection.magnitude) * Mathf.Rad2Deg);
        transform.Rotate(0,0,angle * 0.05f);
        //Debug.Log("Angle: " + angle);
        
    }
}
