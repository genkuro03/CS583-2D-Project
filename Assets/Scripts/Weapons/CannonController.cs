using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : WeaponController

{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); //calls parent fn
    }

    // Update is called once per frame
    protected override void Attack()
    {
        base.Attack();  //calls parent fn
        GameObject spawnedBall = Instantiate(weaponData.Prefab);
        spawnedBall.transform.position = transform.position;    //Spawn cannon ball

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);    //grab mouse pos n direction
        Vector3 direction = (mousePosition - transform.position).normalized;

        spawnedBall.GetComponent<BallBehavior>().SetDirection(direction);   //set direction to mouse dir
    }
}
