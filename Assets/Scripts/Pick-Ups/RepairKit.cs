using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : MonoBehaviour, ICollectible
{
    public int healthToRestore; //Stores health grabbed by pick up

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();   //Finds player obj
        player.RestoreHealth(healthToRestore);  //Calls restore health fn
    }

    private void OnTriggerEnter2D(Collider2D col)   //Checks if collided with player and deletes it.
    {
        if (col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
