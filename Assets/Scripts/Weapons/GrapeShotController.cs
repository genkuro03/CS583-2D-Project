using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeShotController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Attack()
    {
        base.Attack();

        // Define shot count and angles based on weapon level
        int shotCount;
        float[] angles;

        // Access the weapon level from weaponData
        int weaponLevel = weaponData.Level;

        if (weaponLevel == 1)
        {
            shotCount = 1;
            angles = new float[] { 0 }; // Single shot straight forward
        }
        else if (weaponLevel == 2)
        {
            shotCount = 3;
            angles = new float[] { -30f, 0f, 30f }; // Spread shot at 30, 90, 120 degrees
        }
        else if (weaponLevel == 3)
        {
            shotCount = 5;
            angles = new float[] { -45f, -15f, 0f, 15f, 45f }; // Wider spread for 5 shots
        }
        else
        {
            return; // If weapon level is invalid, do nothing
        }

        // Calculate direction for each shot and instantiate projectiles
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 baseDirection = (mousePosition - transform.position).normalized;

        for (int i = 0; i < shotCount; i++)
        {
            // Calculate the rotation angle for each shot
            float angle = angles[i];
            Vector3 direction = Quaternion.Euler(0, 0, angle) * baseDirection;

            // Instantiate the projectile and set its direction
            GameObject spawnedBall = Instantiate(weaponData.Prefab);
            spawnedBall.transform.position = transform.position;
            spawnedBall.GetComponent<BallBehavior>().SetDirection(direction);
        }
    }
}
