using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player; //Grabs player 
    CircleCollider2D playerCollector;   //references player collector
    public float pullSpeed; 

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();   //Grabs player
        playerCollector = GetComponent<CircleCollider2D>(); //Grabs collector
    }

    void Update()
    {
        playerCollector.radius = player.CurrentMagnet;  //Changes radius to player radius (magnet = radisu pretty much)
    }

    void OnTriggerEnter2D(Collider2D col)   //When collide with collectible, pull towards player before deleting
    {
        if(col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - col.transform.position).normalized;
            rb.AddForce(forceDirection * pullSpeed);

            collectible.Collect();
        }
    }
}
