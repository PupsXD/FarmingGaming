using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CombatEntity : MonoBehaviour
{
	[SerializeField] private EntityData _data;

	protected string entityName;
	protected int entityLevel;


	//[Header("Base Stats")]
	protected float maxHealth;
	protected float currentHealth;

	protected float maxMana;
	protected float currentMana;

	protected float dodgeChance;


	//[Header("Battle Stats")]
	protected float damage;
	protected float abilityDamageMultiplier;
	protected float manaPerAttackGain;

	[SerializeField] private UnityEvent HitEvent;
	[SerializeField] private UnityEvent DodgeEvent;
	[SerializeField] private UnityEvent DeadEvent;
	[SerializeField] private UnityEvent AttackEvent;

	#region Accessors
	public string Name
	{
		get { return this.entityName; }
		set { this.entityName = value; }
	}
	public int Level
	{
		get { return this.entityLevel; }
		set { this.entityLevel = value; }
	}
	public float CurrentDodje
	{
		get { return this.dodgeChance; }
		set { this.dodgeChance = value; }
	}
	public float MaxHP
    {
		get { return this.maxHealth; }
		set { this.maxHealth = value; }
	}
	public float CurrentHP
	{
		get { return this.currentHealth; }
		set { this.currentHealth = Mathf.Min(maxHealth, value); }
	}
	public float MaxMana
	{
		get { return this.maxMana; }
		set { this.maxMana = value; }
	}
	public float CurrentMana
	{
		get { return this.currentMana; }
		set { this.currentMana = Mathf.Min(maxMana, value); }
	}

	public float Damage
	{
		get { return this.damage; }
		set { this.damage = value; }
	}

    #endregion Accessors

    private void Awake()
    {
		entityName = _data.entityName;
		entityLevel = _data.entityLevel;
		maxHealth = _data.maxHealth;
		maxMana = _data.maxMana;
		dodgeChance = _data.dodgeChance;
		damage = _data.damage;
		abilityDamageMultiplier = _data.abilityDamageMultiplier;
		manaPerAttackGain = _data.manaPerAttackGain;
		currentHealth = maxHealth;
		currentMana = 0;
    }

    protected virtual void Start()
    {
		GetComponent<SpriteRenderer>().sprite = _data.entitySprite;
		
    }

	public void Attack()
    {
		AttackEvent.Invoke();
    }


    public string TakeDamage(float dmg)
	{
		var range = Random.Range(1f, 101f);

		if (range <= dodgeChance)
        {
			DodgeEvent.Invoke();
			return "dodge";
        }
        else
        {
			currentHealth -= dmg;

			if (currentHealth <= 0)
            {
				DeadEvent.Invoke();
				return "dead";
			}
            else
            {
				HitEvent.Invoke();
				return "hit";
			}	
		}
	}

	public string TakeDamage(float dmg, int aa)
    {
		currentHealth -= dmg * abilityDamageMultiplier;

		if (currentHealth <= 0)
		{
			DeadEvent.Invoke();
			return "dead";
		}
		else
		{
			HitEvent.Invoke();
			return "hit";
		}
	}

	public void AddMana()
    {
		CurrentMana += manaPerAttackGain;
	}

}
