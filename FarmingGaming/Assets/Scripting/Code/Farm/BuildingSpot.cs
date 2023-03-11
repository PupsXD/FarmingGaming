using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpot : MonoBehaviour
{
    [SerializeField] private GameObject _interactionTooltip;
    [SerializeField] private BuildingSpotContextMenu _contextMenu;
    [SerializeField] private GameObject _zone;

    [SerializeField] private FoodSpot _foodSpotPrefab;
    [SerializeField] private Pound _poundPrefab;
    private bool _playerIsNear;
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
                PlayerPrefs.GetString(string.Format("BuildingSpot-{0}-type", transform.position.ToString().GetHashCode()))
            );
    }

    private void LoadBuilding(string buildingType)
    {
        switch (buildingType)
        {
            case "food":
                BuildFoodSpot();
                break;
            case "pound":
                BuildPound();
                break;
        }
    }

    public void BuildFoodSpot()
    {
        if (!Empty) return;
        _building = Instantiate(_foodSpotPrefab, transform.position, Quaternion.identity, transform);
        CloseContextMenu();
        PlayerPrefs.SetString(string.Format("BuildingSpot-{0}-type", transform.position.ToString().GetHashCode()), "food");
    }
    public void BuildPound()
    {
        if (!Empty) return;

        _building = Instantiate(_poundPrefab, transform.position, Quaternion.identity, transform);

        CloseContextMenu();
        PlayerPrefs.SetString(string.Format("BuildingSpot-{0}-type", transform.position.ToString().GetHashCode()), "pound");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _interactionTooltip.SetActive(true);
            _playerIsNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerIsNear = false;
            _interactionTooltip.SetActive(false);
            CloseContextMenu();
        }
    }
    private void Update()
    {
        if (_playerIsNear)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                
                OpenContextMenu();
            }
        }
    }


    private void OpenContextMenu()
    {
        _interactionTooltip.SetActive(false );
        _contextMenu.ChooseAvailableOptions(this);
        _contextMenu.SetActive(true);
    }

    private void CloseContextMenu()
    {
        _contextMenu.SetActive(false);
    }
}
