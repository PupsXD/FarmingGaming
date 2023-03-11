using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSpotUI : MonoBehaviour
{
    [SerializeField] private FoodSpot _foodSpot;
    [SerializeField] private Button _collectButton;
    [SerializeField] private Image _fillImage;
    private void Awake()
    {
        _collectButton.onClick.AddListener(TryCollect);
    }

    private void Update()
    {
        if(_collectButton.gameObject.activeSelf)
            _fillImage.fillAmount = _foodSpot.NormalizedGrowthTime;
    }
    private void TryCollect()
    {
        if (!_foodSpot.Ready) return;
        _foodSpot.CollectHarvest();
    }
}
