using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class LootData
{
    public GameObject lootPrefab;  // Reference to the loot prefab
    public float spawnChance;      // Probability of this loot spawning (0 to 1)
}


public class LootSpawner : MonoBehaviour    //Responsible to manage drops around player
{
    public LootData[] lootItems;        // Array to hold loot items with different spawn chances
    public float spawnRadius = 20f;     // Radius around the player to spawn loot
    public float spawnInterval = 5f;    // Time interval between each spawn

    private Transform player;
    private Tilemap collisionTilemap;   //Reference to tilemap (mainly border)

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  //Grabs player

        GameObject borderTilemapObject = GameObject.FindGameObjectWithTag("Border");    //Finds tilemap marked by border
        if (borderTilemapObject != null)
        {
            collisionTilemap = borderTilemapObject.GetComponent<Tilemap>(); 
        }

        StartCoroutine(SpawnLootPeriodically());    //Starts coroutine to spawn loot periodically
    }

    IEnumerator SpawnLootPeriodically() //Spawn loot at set intervals
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); //waits specified interval
            SpawnLoot();    //Spawns loot
        }
    }

    void SpawnLoot()
    {
        if (player == null || collisionTilemap == null) return;

        // Select a loot item based on spawn chances
        GameObject lootPrefab = GetRandomLootPrefab();
        if (lootPrefab == null) return;

        // Loop until a valid spawn position is found
        Vector3 spawnPosition;
        int attempts = 0;
        do
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized * spawnRadius;
            spawnPosition = player.position + new Vector3(randomDirection.x, randomDirection.y, 0);
            attempts++;
        } while (!IsValidSpawnPosition(spawnPosition) && attempts < 10); // Limit attempts to avoid infinite loops

        if (IsValidSpawnPosition(spawnPosition))
        {
            Instantiate(lootPrefab, spawnPosition, Quaternion.identity);
        }
    }

    //Method to randomly select loot item based on spawn chances
    GameObject GetRandomLootPrefab()
    {
        float totalChance = 0f; //# of attempts until it gives up
        foreach (var loot in lootItems)
        {
            totalChance += loot.spawnChance;
        }

        // Roll a random number between 0 and the total spawn chance
        float randomRoll = Random.Range(0, totalChance);
        float cumulativeChance = 0f;

        foreach (var loot in lootItems)
        {
            cumulativeChance += loot.spawnChance;
            if (randomRoll <= cumulativeChance)
            {
                return loot.lootPrefab; //Returns the selected loot prefab
            }
        }

        return null; // In case something goes wrong
    }

    bool IsValidSpawnPosition(Vector3 position) //Checks if spawn pos is valid
    {
        Vector3Int cellPosition = collisionTilemap.WorldToCell(position);   //Converts world pos to tilemap cell pos
        return collisionTilemap.GetTile(cellPosition) == null;  //Checks if no collision tilemap
    }
}
