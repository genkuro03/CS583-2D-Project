using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class EnemyGroup //Organizes the enemy group (specific enemies)
{
    public string enemyName;
    public int enemyCount;  //How many are spawned
    public int spawnCount;  //How mnay to  spawn
    public GameObject enemyPrefab;  //enemy prefab (enemy sailboat...)
}

[System.Serializable]
public class Wave   //Sets up waves, dont really use it in mine since very low scaled
{
    public string waveName;
    public List<EnemyGroup> enemyGroups;
    public int waveQuota;
    public float spawnInterval;
    public int spawnCount;
    public int currentGroupIndex; // Track the current enemy group within the wave
}

public class EnemySpawner : MonoBehaviour
{
    public List<Wave> waves;
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    float spawnTimer;
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached = false;
    public float waveInterval;

    [Header("Spawn Positions")]     //Set up fixed spawn pos around player, so they dont spawn on top of player
    public float spawnRadius = 20f; //Also checks if spawning on collision tile map
    public LayerMask obstacleLayer;
    private Transform player;
    private Tilemap collisionTilemap;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  //Finds player,

        GameObject borderTilemapObject = GameObject.FindGameObjectWithTag("Border");    //Finds border
        if (borderTilemapObject != null)
        {
            collisionTilemap = borderTilemapObject.GetComponent<Tilemap>();
        }

        CalculateWaveQuota();   //Calculates waves
    }

    void Update()
    {
        //Check if the current wave is complete and ready to move to the next wave
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount >= waves[currentWaveCount].waveQuota && enemiesAlive == 0)
        {
            StartCoroutine(BeginNextWave());
        }

        //Spawn enemies within the current wave
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)    //Spawns enemies when interval reached 
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave() //Function to begin next wave
    {
        yield return new WaitForSeconds(waveInterval);  //Waits wave interval (very minimal in my game)

        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            waves[currentWaveCount].currentGroupIndex = 0; //Reset to the first enemy group for the new wave
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()   
    {
        int currentWaveQuota = 0;   //Eveyr wave quota starts 0
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;  //Grabs each enemyGroup and add to total (all groups = quota)
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning("Wave Quota for " + waves[currentWaveCount].waveName + ": " + currentWaveQuota);
    }

    void SpawnEnemies()
    {
        //Only proceed if the current wave has remaining enemies to spawn and maxEnemies limit isn't reached
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            var currentWave = waves[currentWaveCount];
            var currentEnemyGroup = currentWave.enemyGroups[currentWave.currentGroupIndex];

            //Spawn enemies from the current enemy group until its quota is fulfilled
            if (currentEnemyGroup.spawnCount < currentEnemyGroup.enemyCount)
            {
                if (enemiesAlive >= maxEnemiesAllowed)
                {
                    maxEnemiesReached = true;
                    return;
                }

                //Try to find a valid spawn point that is not obstructed
                Vector3 spawnPosition = Vector3.zero;
                int attempts = 0;
                int maxAttempts = 10;

                do
                {
                    Vector2 randomDirection = Random.insideUnitCircle.normalized * spawnRadius;
                    spawnPosition = player.position + new Vector3(randomDirection.x, randomDirection.y, 0);
                    attempts++;
                } while (!IsValidSpawnPosition(spawnPosition) && attempts < maxAttempts);   //Give up if can't find spawn pos in reasonable time

                if (IsValidSpawnPosition(spawnPosition))    //Check if valid pos 
                {
                    Instantiate(currentEnemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);
                    currentEnemyGroup.spawnCount++;
                    currentWave.spawnCount++;
                    enemiesAlive++;
                }
            }
            else
            {
                //Move to the next enemy group within the wave if the current group is fully spawned
                if (currentWave.currentGroupIndex < currentWave.enemyGroups.Count - 1)
                {
                    currentWave.currentGroupIndex++;
                }
            }
        }

        // Reset maxEnemiesReached flag if enemies are below max limit
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    bool IsValidSpawnPosition(Vector3 position) //Helper to check if collision w/ tile map
    {
        if (collisionTilemap == null) return true;

        Vector3Int cellPosition = collisionTilemap.WorldToCell(position);
        return collisionTilemap.GetTile(cellPosition) == null; // Check if there's no tile in this position
    }

    public void OnEnemyKilled() //When enemy killed
    {
        enemiesAlive--;

        //If all enemies are defeated, check if the wave is complete
        if (enemiesAlive == 0 && waves[currentWaveCount].spawnCount >= waves[currentWaveCount].waveQuota)
        {
            Debug.Log("Wave " + waves[currentWaveCount].waveName + " completed!");
        }
    }
}
