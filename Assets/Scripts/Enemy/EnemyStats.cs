using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    public EnemyScriptableObject enemyData; //Grabs reference to enemyData  
    
    //Enemy Stats, hid to make inspector bearable to go thru
    [HideInInspector]
    public float currentMoveSpeed;  
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    [Header("Audio Clips")]
    public AudioClip spawnSound;
    public AudioClip deathSound; //Audio clip for when the enemy is destroyed and spawned
    private AudioSource audioSource; 

    void Awake()    //On spawn assign stats
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;

        audioSource = gameObject.AddComponent<AudioSource>();   //Grabs audio sources
        audioSource.playOnAwake = false;

        if (spawnSound != null) //Trigger spawn sound
        {
            audioSource.PlayOneShot(spawnSound, 0.1f);

        }

    }

    public void TakeDamage(float dmg)   //decrements health and checks if dead
    {
        currentHealth -= dmg;

        if(currentHealth <= 0)
        {
            Kill();
        }
    }
        
    public void Kill()  //Helper kill function to destory enemy (and play death sound)
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        Destroy(gameObject);
    }



    private void OnCollisionStay2D(Collision2D col) //Collision checker w/ player, if collide damage player
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnDestroy()    //When enemy destoryed
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>(); //Tell spawner that enemy died (frees up slot for new enemy)
        es.OnEnemyKilled();
    }
}
    
