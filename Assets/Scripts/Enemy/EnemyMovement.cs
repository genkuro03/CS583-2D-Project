using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;
    public LayerMask obstacleLayer;

    private Vector2 lastPosition;
    private float stuckTime;
    private float stuckThreshold = 5f;   //In case they get stuck... but broke at some point    
    private float despawnTime = 5f;      //Rest is pretty self explanitory
    private Rigidbody2D rb;


    // Directional sprites
    public Sprite northSprite;
    public Sprite northEastSprite;
    public Sprite eastSprite;
    public Sprite southEastSprite;
    public Sprite southSprite;
    public Sprite southWestSprite;
    public Sprite westSprite;
    public Sprite northWestSprite;

    private SpriteRenderer sr;

    void Start()
    {
        enemy = GetComponent<EnemyStats>(); //Grabs enemy (self) states
        player = FindObjectOfType<PlayerMovement>().transform;  //Finds player and tracks where it is
        rb = GetComponent<Rigidbody2D>();   
        lastPosition = transform.position;
        stuckTime = 0f;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()   
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
        //Tracks player and moves directly to wherever player is

        UpdateSpriteDirection();    //Updates direction based on player transformation 
        CheckIfStuck(); //Checks if enemy stuck

    }

    void UpdateSpriteDirection()
    {
        Vector2 direction = (player.position - transform.position).normalized;  //Changes self direction to player

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Assign the appropriate sprite based on the angle
        if (angle >= 67.5f && angle < 112.5f)
            sr.sprite = northSprite;          // North
        else if (angle >= 22.5f && angle < 67.5f)
            sr.sprite = northEastSprite;      // North-East
        else if (angle >= -22.5f && angle < 22.5f)
            sr.sprite = eastSprite;           // East
        else if (angle >= -67.5f && angle < -22.5f)
            sr.sprite = southEastSprite;      // South-East
        else if (angle >= -112.5f && angle < -67.5f)
            sr.sprite = southSprite;          // South
        else if (angle >= -157.5f && angle < -112.5f)
            sr.sprite = southWestSprite;      // South-West
        else if (angle >= 112.5f && angle < 157.5f)
            sr.sprite = northWestSprite;      // North-West
        else
            sr.sprite = westSprite;           // West
    }

    void CheckIfStuck()
    {
        // Check if the enemy has moved a certain distance
        float distanceMoved = Vector2.Distance(transform.position, lastPosition);
        if (distanceMoved < stuckThreshold)
        {
            stuckTime += Time.deltaTime;
        }
        else
        {
            // Reset the stuck timer if the enemy moved
            stuckTime = 0f;
            lastPosition = transform.position;
        }

        // Despawn or respawn the enemy if stuck for too long
        if (stuckTime >= despawnTime)
        {
            Debug.Log("Enemy despawned due to being stuck.");
            Destroy(gameObject);  // Or implement respawn logic here
        }
    }

}
