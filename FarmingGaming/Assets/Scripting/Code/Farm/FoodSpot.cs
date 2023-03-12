using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodSpot : Building
{
    [SerializeField] private GameObject _collectButton;
    [SerializeField] private ItemsSubLib _foodLib;
    public override int MaxUpgradeLevel => 1;
    private Food _selectedFood;
    private DateTime _growthStartAt;
    public float NormalizedGrowthTime
    {
        get
        {
            return ((float)DateTime.Now.Subtract(_growthStartAt).TotalSeconds) / (float)_selectedFood.GrowthTime.TotalSeconds;
        }
    }
    public bool Ready
    {
        get
        {
            return NormalizedGrowthTime >= 1;
        }
    }

    private void Start()
    {
        LoadSave();
    }

    private void LoadSave()
    {
        int selectedFoodID = PlayerPrefs.GetInt(string.Format("FoodSpot-{0}-foodID", transform.position.ToString().GetHashCode()), -1);
        if(selectedFoodID < 0)
        {
            AutoSelectCarrot();
        }
        else
        {
            _selectedFood = _foodLib.Items.First((f) => f.ItemID == selectedFoodID) as Food;
            _growthStartAt = DateTime.Parse(PlayerPrefs.GetString(string.Format("FoodSpot-{0}-startTime", transform.position.ToString().GetHashCode())));
        }
    }

    private void AutoSelectCarrot()
    {
        _selectedFood = _foodLib[0] as Food;
        _growthStartAt = DateTime.Now;
        PlayerPrefs.SetInt(string.Format("FoodSpot-{0}-foodID", transform.position.ToString().GetHashCode()), _selectedFood.ItemID);
        PlayerPrefs.SetString(string.Format("FoodSpot-{0}-startTime", transform.position.ToString().GetHashCode()), _growthStartAt.ToString());
    }

    public void CollectHarvest()
    {

        Inventory.Instance.TryAdd(_selectedFood, _selectedFood.ItemsPerOneGrowthCycle);

        //restart
        _growthStartAt = DateTime.Now;
        PlayerPrefs.SetString(string.Format("FoodSpot-{0}-startTime", transform.position.ToString().GetHashCode()), _growthStartAt.ToString());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _collectButton.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _collectButton.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        if (_collectButton.activeSelf)
        {
            _collectButton.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }
}
