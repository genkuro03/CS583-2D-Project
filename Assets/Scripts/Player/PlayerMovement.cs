using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 moveDir;

    Rigidbody2D rb;
    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();   //Grabs player
        rb = GetComponent<Rigidbody2D>();   //Grabs Rigid2D
    }

    //Update is called once per frame
    void Update()
    {
        InputManagement(); //Grabs new movement / stores old
    }

    void FixedUpdate()
    {
        Move();     //Calls movement function
    }

    void InputManagement()
    {
        if (GameManager.instance.isGameOver)    //Cheks if game is over (prevents sprite from moving after time freeze)
        {
            return; //Skip
        }

        float moveX = Input.GetAxisRaw("Horizontal");   //Grabs x
        float moveY = Input.GetAxisRaw("Vertical"); //Grabs y

        moveDir = new Vector2(moveX, moveY).normalized; //creates 2D vector to move

        if(moveDir.x != 0)  //Checks if x is non 0
        {
            lastHorizontalVector = moveDir.x;
        }
        if(moveDir.y != 0)  //Check if y is non 0
        {
            lastVerticalVector = moveDir.y;
        }
    }

    void Move()
    {
        if (GameManager.instance.isGameOver)    //Cheks if game is over (prevents sprite from moving after time freeze)
        {
            return; //Skip
        }
        rb.velocity = new Vector2(moveDir.x * player.CurrentMoveSpeed, moveDir.y * player.CurrentMoveSpeed);    //Changes new velocity after
    }
}
