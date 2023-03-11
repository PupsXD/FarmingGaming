using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRandomizer : MonoBehaviour
{
    public List<GameObject> liGoSpawn = new List<GameObject>();
    public List<GameObject> tilesGrid = new List<GameObject>();
    

    void Start()
    {
        GridRandomizer();
    }


    public void GridRandomizer()
    {
        int f = tilesGrid.Count;
        for (int i = 0; i < f; i++)
        {

            Vector3 tilePos = tilesGrid[i].transform.position;


            GameObject goToSpawn = liGoSpawn[Random.Range(0, liGoSpawn.Count)];


            Instantiate(goToSpawn, tilePos, Quaternion.identity);


            Destroy(tilesGrid[i]);
        }
    }
}
