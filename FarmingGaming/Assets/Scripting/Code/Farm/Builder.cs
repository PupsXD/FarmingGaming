using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Builder : MonoBehaviour
{
    [SerializeField] private Building[] _allBuildings;
    [SerializeField] private Button _buildButton;
    [SerializeField] private Image _builderAim;
    [SerializeField] private LayerMask _whatIsBuildingSpot;
    private bool _inPlacingMode;
    private int _selectedBuildingIndex;

    private void Awake()
    {
        PrepareUI();
    }

    private void PrepareUI()
    {
        for (int i = 0; i < _allBuildings.Length; i++)
        {
            if (i == 0)
            {
                int index = i;
                _buildButton.onClick.AddListener(() => TryBuildByIndex(index));
                _buildButton.transform.GetChild(0).GetComponent<Image>().sprite = _allBuildings[i].BuildingIcon;
            }
            else
            {
                Button button = Instantiate(_buildButton, _buildButton.transform.parent);
                button.onClick.RemoveAllListeners();
                int index = i;
                button.onClick.AddListener(() => TryBuildByIndex(index));
                button.transform.GetChild(0).GetComponent<Image>().sprite = _allBuildings[i].BuildingIcon;
            }
        }
    }

    private void TryBuildByIndex(int index)
    {
        _inPlacingMode = true;
        _builderAim.sprite = _allBuildings[index].BuildingIcon;
        _builderAim.gameObject.SetActive(true);
        _selectedBuildingIndex = index;
    }
    public void CloseBuildStream()
    {
        _inPlacingMode = false;
        _builderAim.gameObject.SetActive(false);
        _selectedBuildingIndex = -1;
    }

    private void Update()
    {
        if (_inPlacingMode)
        {
            _builderAim.transform.position = Input.mousePosition;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,Mathf.Infinity, _whatIsBuildingSpot);

            if (hit.collider != null)
            {

                if (hit.transform.TryGetComponent(out BuildingSpot buildingSpot))
                {
                    if (buildingSpot.Empty)
                    {
                        _builderAim.color = new Color(1, 1, 1, 145f / 256f); ;

                        if (Input.GetMouseButtonDown(0))
                        {
                            buildingSpot.Build(_allBuildings[_selectedBuildingIndex]);
                            CloseBuildStream();
                        }
                    }
                }
                else
                    _builderAim.color = new Color(1,0,0,145f/256f);
            }
            else
                _builderAim.color = new Color(1, 0, 0, 145f / 256f);
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseBuildStream();
        }
    }

    public Building GetBuildingByName(string buildingName)
    {
        return _allBuildings.First((b) => b.name == buildingName);
    }
}

