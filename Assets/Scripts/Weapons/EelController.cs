using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();   //Parent fn
    }

    protected override void Attack()
    {
        base.Attack();//Parent fn
        GameObject spawnedEels = Instantiate(weaponData.Prefab);    //Spawns eels
        spawnedEels.transform.position = transform.position;
        spawnedEels.transform.parent = transform;
    }
}
