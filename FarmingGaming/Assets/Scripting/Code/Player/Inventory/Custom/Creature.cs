using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Item
{
    [SerializeField] private Food _prefferedFood;
    [SerializeField] private int _requiredFoodAmountToFeed;
    public Food PrefferedFood
    {
        get { return _prefferedFood; }
    }
    public int RequiredFoodAmountToFeed
    {
        get { return _requiredFoodAmountToFeed;}
    }
}
