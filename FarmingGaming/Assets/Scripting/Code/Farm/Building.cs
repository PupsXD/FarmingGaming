using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [SerializeField] private Sprite _buildingIcon;
    [SerializeField] private int _buildCost = 3;
    public Sprite BuildingIcon
    {
        get { return _buildingIcon; }
    }
    public int BuildCost
    {
        get { return _buildCost; }
    }
    private bool _isBroken;
    protected int _currentUpgradeLevel = 1;
    public bool IsBroken
    {
        get { return _isBroken; }
    }
    public int CurrentUpgradeLevel
    {
        get { return _currentUpgradeLevel; }
    }
    public abstract int MaxUpgradeLevel { get; }
}
