using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : CombatEntity
{
    [Header("Ability Turns Duration")]
    [SerializeField] protected int turnsDuration;

    private float baseDodgeChance;

    protected override void Start()
    {
        base.Start();
        baseDodgeChance = dodgeChance;
    }
    public int DodgeAbilityON(int currentTurn)
    {
        dodgeChance = 101;

        return currentTurn + turnsDuration;
    }

    public void DodgeAbilityOFF()
    {
        dodgeChance = baseDodgeChance;
    }
}
