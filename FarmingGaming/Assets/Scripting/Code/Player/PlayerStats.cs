using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player stats")]
public class PlayerStats : Stats
{
    [SerializeField] private int _maxHealth = 5;
    [SerializeField] private float _moveSpeed = 1.0f;
    public override int MaxHealth
    {
        get { return _maxHealth * 1; }
    }

    public override float MoveSpeed
    { 
        get { return _moveSpeed * 1; }
    }
}
