using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventGeneration : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Vector2 spawnZonePosition;
    public GameObject spawnZone;
    public float spawnZoneMinX;
    public float spawnZoneMaxX;
    public float spawnZoneMinY;
    public float spawnZoneMaxY;

    void Start()
    {
        spawnZoneMinX = spawnZonePosition.x - (spawnZone.transform.localScale.x / 2);
        spawnZoneMaxX = spawnZonePosition.x + (spawnZone.transform.localScale.x / 2);
        spawnZoneMinY = spawnZonePosition.y - (spawnZone.transform.localScale.y / 2);
        spawnZoneMaxY = spawnZonePosition.y + (spawnZone.transform.localScale.y / 2);
        
        for (int i = 0; i < Random.Range(3, 15); i++)
        {
            SpawnObject();
        }
        Destroy(spawnZone);
    }

    // void Update()
    // {
    //     
    // }

    void SpawnObject()
    {
        float spawnX = Random.Range(spawnZoneMinX, spawnZoneMaxX);
        float spawnY = Random.Range(spawnZoneMinY, spawnZoneMaxY);

        Instantiate(prefabToSpawn, new Vector2(spawnX, spawnY), Quaternion.identity);
    }
}
