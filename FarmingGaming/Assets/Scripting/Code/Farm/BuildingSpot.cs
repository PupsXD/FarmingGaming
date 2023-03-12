using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BuildingSpot : MonoBehaviour
{
    [SerializeField] private GameObject _zone;
    private Building _building;

    public bool Empty
    {
        get { return _building == null; }
    }
    public Building Building
    {
        get { return _building; }
    }

    private void Awake()
    {
        LoadBuilding
            (
                PlayerPrefs.GetString(string.Format("BuildingSpot-{0}-name", transform.position.ToString().GetHashCode()))
            );
    }

    private void LoadBuilding(string buildingName)
    {
        if(buildingName.Length > 0)
         Build(FindObjectOfType<Builder>().GetBuildingByName(buildingName));
    }


    public void Build(Building building)
    {
        if (!Empty) return;
        _building = Instantiate(building, transform.position, Quaternion.identity, transform);
        PlayerPrefs.SetString(string.Format("BuildingSpot-{0}-name", transform.position.ToString().GetHashCode()), building.name);
        GetComponent<Collider2D>().enabled = false;
        _zone.SetActive(false);
    }
}
