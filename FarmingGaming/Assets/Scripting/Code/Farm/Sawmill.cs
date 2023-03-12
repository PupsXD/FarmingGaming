using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawmill : Building
{
    [SerializeField] private GameObject _collectButton;
    [SerializeField] private float _productionLengthInMinutes = 1;
    [SerializeField] private int _itemsPerProductionCycle = 10;
    [SerializeField] private BuildingResource _product;
    public override int MaxUpgradeLevel => 1;
    private DateTime _productionStartAt;

    public float NormalizedProductionTime
    {
        get
        {
            return ((float)DateTime.Now.Subtract(_productionStartAt).TotalSeconds) / (_productionLengthInMinutes*60);
        }
    }
    public bool Ready
    {
        get
        {
            return NormalizedProductionTime >= 1;
        }
    }

    private void Awake()
    {
        LoadSave();
        //_collectButton.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void LoadSave()
    {
        _currentUpgradeLevel = PlayerPrefs.GetInt(string.Format("Sawmill-{0}-level", transform.position.ToString().GetHashCode()), 1);
        string startTime = PlayerPrefs.GetString(string.Format("Sawmill-{0}-startTime", transform.position.ToString().GetHashCode()));
        if(startTime.Length > 0)
        {
            _productionStartAt = DateTime.Parse(startTime);
        }
        else
        {
            _productionStartAt = DateTime.Now;
            PlayerPrefs.SetString(string.Format("Sawmill-{0}-startTime", transform.position.ToString().GetHashCode()), _productionStartAt.ToString());
        }
    }
    public void CollectProductionResult()
    {

        Inventory.Instance.TryAdd(_product, _itemsPerProductionCycle);

        //restart
        _productionStartAt = DateTime.Now;
        PlayerPrefs.SetString(string.Format("Sawmill-{0}-startTime", transform.position.ToString().GetHashCode()), _productionStartAt.ToString());
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

}
