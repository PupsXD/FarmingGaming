using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : CombatEntity
{
    [Header("Ability Turns Duration")]
    [SerializeField] protected int turnsDuration;
    [SerializeField] private ParticleSystem evadeParcicles;
    private float baseDodgeChance;

    protected override void Start()
    {
        base.Start();
        baseDodgeChance = dodgeChance;
    }
    public int DodgeAbilityON(int currentTurn)
    {
        dodgeChance = 101;
        evadeParcicles.Play();
        return currentTurn + turnsDuration;
    }

    public void DodgeAbilityOFF()
    {
        evadeParcicles.Stop();
        dodgeChance = baseDodgeChance;
    }
}
