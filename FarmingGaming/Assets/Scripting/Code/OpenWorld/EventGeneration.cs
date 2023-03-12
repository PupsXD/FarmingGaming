using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventGeneration : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public GameObject prefabToSpawn;
    public Vector2 spawnZonePosition;
    public GameObject spawnZone;
    public float spawnZoneMinX;
    public float spawnZoneMaxX;
    public float spawnZoneMinY;
    public float spawnZoneMaxY;

    void Awake()
    {
        spawnZoneMinX = spawnZonePosition.x - (spawnZone.transform.localScale.x / 2);
        spawnZoneMaxX = spawnZonePosition.x + (spawnZone.transform.localScale.x / 2);
        spawnZoneMinY = spawnZonePosition.y - (spawnZone.transform.localScale.y / 2);
        spawnZoneMaxY = spawnZonePosition.y + (spawnZone.transform.localScale.y / 2);
        
        for (int i = 0; i < Random.Range(3, 15); i++)
        {
            int r = Random.Range(0, 2);
            if (r > 0)
            {
                SpawnEnemy();
            }
            else
            {
                SpawnObject();
            }
            
        }
        Destroy(spawnZone);
    }
    
    void SpawnObject()
    {
        // Define the spawning area
        Vector2 spawnPosition = new Vector2(Random.Range(spawnZoneMinX, spawnZoneMaxX), Random.Range(spawnZoneMinY, spawnZoneMaxY));
        Vector2 spawnSize = new Vector2(prefabToSpawn.transform.localScale.x, prefabToSpawn.transform.localScale.y);

        // Check if there are any colliders within the spawning area
        Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnPosition, spawnSize, 0);

        // If there are any colliders, find a new spawn position
        while (colliders.Length > 0)
        {
            spawnPosition = new Vector2(Random.Range(spawnZoneMinX, spawnZoneMaxX), Random.Range(spawnZoneMinY, spawnZoneMaxY));
            colliders = Physics2D.OverlapBoxAll(spawnPosition, spawnSize, 0);
        }

        // Spawn the object at the spawn position
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
    
    void SpawnEnemy()
    {
        // Define the spawning area
        Vector2 spawnPosition = new Vector2(Random.Range(spawnZoneMinX, spawnZoneMaxX), Random.Range(spawnZoneMinY, spawnZoneMaxY));
        Vector2 spawnSize = new Vector2(enemyToSpawn.transform.localScale.x, enemyToSpawn.transform.localScale.y);

        // Check if there are any colliders within the spawning area
        Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnPosition, spawnSize, 0);

        // If there are any colliders, find a new spawn position
        while (colliders.Length > 0)
        {
            spawnPosition = new Vector2(Random.Range(spawnZoneMinX, spawnZoneMaxX), Random.Range(spawnZoneMinY, spawnZoneMaxY));
            colliders = Physics2D.OverlapBoxAll(spawnPosition, spawnSize, 0);
        }

        // Spawn the object at the spawn position
        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
    }
    // void SpawnObject()
    // {
    //     
    //     float spawnX = Random.Range(spawnZoneMinX, spawnZoneMaxX);
    //     float spawnY = Random.Range(spawnZoneMinY, spawnZoneMaxY);
    //
    //     Instantiate(prefabToSpawn, new Vector2(spawnX, spawnY), Quaternion.identity);
    // }
}
