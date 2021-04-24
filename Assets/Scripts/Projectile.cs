using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed;
    public float lifeTime;
    public float enemyKnockback;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        //ignore collision with player
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<PolygonCollider2D>());
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = transform.right * speed;
        
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector2.up * speed * Time.deltaTime);
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (!other.CompareTag("Enemy"))
        // {
        //     // Rigidbody2D enemyRb2d = other.GetComponent<Rigidbody2D>();
        //     // Vector3 dir = other.transform.position - transform.position;
        //     // float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //     // AddForceAtAngle(enemyKnockback, angle, enemyRb2d);
        //
        //     
        // }
        // Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject, 0.001f);
    }

    public void AddForceAtAngle(float force, float angle, Rigidbody2D rigidbody)
    {
        float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * force;
        float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * force;
 
        rigidbody.AddForce(new Vector2(xcomponent, ycomponent), ForceMode2D.Impulse);
    }
}
