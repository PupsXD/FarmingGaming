using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
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
