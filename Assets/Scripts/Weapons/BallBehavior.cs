using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : ProjectileWeaponBehavior
{
    private Vector3 fireDirection;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public void SetDirection(Vector3 newDirection)
    {
        fireDirection = newDirection.normalized; // Normalize to ensure consistent speed
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += fireDirection * currentSpeed * Time.deltaTime;    //transformation for ball based on its fields
    }
}
