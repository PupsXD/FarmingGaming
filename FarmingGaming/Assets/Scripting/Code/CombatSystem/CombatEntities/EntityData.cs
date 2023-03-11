using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Entity")]
public class EntityData : ScriptableObject
{
	public string entityName;
	public int entityLevel;

	public Sprite entitySprite;

	public float maxHealth;
	public float maxMana;
	public float dodgeChance;

	public float damage;
	public float abilityDamageMultiplier;
	public float manaPerAttackGain;
}
