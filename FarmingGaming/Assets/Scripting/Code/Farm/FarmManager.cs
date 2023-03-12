using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    private static FarmManager farmManagerInscance;
    /// <summary>
    /// UI
    /// </summary>
    [SerializeField] private TextMeshProUGUI woodCountText;
    [SerializeField] private TextMeshProUGUI foodCountText;

    /// <summary>
    /// Resouces
    /// </summary>

    private int _woodCount;
    private int _foodCount;
    
    public int WoodCount { get => _woodCount; set => _woodCount = value; }
    public int FoodCount { get => _foodCount; set => _foodCount = value; }


    private void Start()
    {
        farmManagerInscance = this;
    }

    public void UpdateResorces(int woodCount, int foodCount)
    {
        _woodCount += woodCount;
        _foodCount += foodCount;
        
        if(woodCountText)
            woodCountText.text = _woodCount.ToString();
        if(foodCountText)
            foodCountText.text = _woodCount.ToString();
    }
}
