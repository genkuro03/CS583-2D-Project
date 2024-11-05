using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGold : MonoBehaviour, ICollectible
{
    public int experienceGranted;   //field to hold experience to add

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();   //finds player obj
        player.IncreaseExperience(experienceGranted);   //Calls increase fn w/ new increase of experience as arg
    }

    private void OnTriggerEnter2D(Collider2D col)   //Checks if xp collided with player
    {
        if (col.CompareTag("Player"))
        {
            Destroy(gameObject);    //Deletes xp
        }
    }
}
