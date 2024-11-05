using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelBehavior : MeleeWeaponBehavior
{

    List<GameObject> markedEnemies;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>(); //List of enemies that have been hit by melee weapon
    }

    protected override void OnTriggerEnter2D(Collider2D col)    //when they enter the collider for melee weapon, damage them.
    {
        if (col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);

            markedEnemies.Add(col.gameObject);  //Makes sure to not double damage enemies 
        }
    }
}
