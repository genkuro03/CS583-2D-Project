using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;   //Inspector so we can assign prefabs and scripts
    float currentCooldown;

    protected PlayerMovement pm;

    [Header("Attack Sound")]
    public AudioClip attackSound; // Add a field for the attack sound
    private AudioSource audioSource; // Audio source component

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = weaponData.CooldownDuration;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = attackSound;
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;  //Calculates how often to fire off weapon
        if(currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack() //Simple function to just set rate of attack
    {
        currentCooldown = weaponData.CooldownDuration;

        if (attackSound != null)
        {
            audioSource.Play();
        }

    }
}
