using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSpotContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject _origin;
    [SerializeField] private Button _buildFoodSpotOption;
    [SerializeField] private Button _buildPoundOption;
    [SerializeField] private Button _upgradeOption;
    [SerializeField] private Button _repairOption;
    [SerializeField] private GameObject _maxLevel;

    private BuildingSpot _aimedSpot;

    private void Awake()
    {
        _buildFoodSpotOption.onClick.AddListener(BuildFoodSpot);
        _buildPoundOption.onClick.AddListener(BuildPound);
    }

    public void SetActive(bool v)
    {
        _origin.SetActive(v);
    }
    public void ChooseAvailableOptions(BuildingSpot spot)
    {
        _buildFoodSpotOption.gameObject.SetActive(false);
        _buildPoundOption.gameObject.SetActive(false);
        _upgradeOption.gameObject.SetActive(false);
        _repairOption.gameObject.SetActive(false);
        _maxLevel.SetActive(false);
        if (spot.Empty)
        {
            _buildFoodSpotOption.gameObject.SetActive(true);
            _buildPoundOption.gameObject.SetActive(true);
        }
        else
        {
            if(spot.Building.IsBroken)
            {
                _repairOption.gameObject.SetActive(true);
            }
            else
            {
                if(spot.Building.CurrentUpgradeLevel < spot.Building.MaxUpgradeLevel)
                {
                    _upgradeOption.gameObject.SetActive(true);
                }
                else
                {
                    _maxLevel.SetActive(true);
                }
            }
        }
        _origin.transform.position = Camera.main.WorldToScreenPoint(spot.transform.position);
        _aimedSpot = spot;
    }

    private void BuildFoodSpot()
    {
        _aimedSpot.BuildFoodSpot();
    }
    private void BuildPound()
    {
        _aimedSpot.BuildPound();
    }
}
