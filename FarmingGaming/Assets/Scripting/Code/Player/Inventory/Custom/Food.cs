using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Food")]
public class Food : Item
{
    public SerializableTimeSpan GrowthTime;
    /// <summary>
    /// Items count from one growth cycle
    /// </summary>
    public int ItemsPerOneGrowthCycle;
}
