using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

	[SerializeField] private Text nameText;
	[SerializeField] private Text levelText;
	[SerializeField] private Slider hpSlider;
	[SerializeField] private Slider manaSlider;
	[SerializeField] private Text hpText;
	[SerializeField] private Text manaText;
	[SerializeField] private Text dodgeText;
	[SerializeField] private Text damageText;
	

	public void SetHUD(CombatEntity entity)
	{
		nameText.text = entity.Name;
		levelText.text = "LEVEL " + entity.Level;
		hpSlider.maxValue = entity.MaxHP;
		hpSlider.value = entity.MaxHP;
		manaSlider.maxValue = entity.MaxMana;
		manaSlider.value = 0;
		dodgeText.text = "Dodge chance - " + entity.CurrentDodje;
		manaText.text = "Energy - " + entity.CurrentMana;
		hpText.text = "HP - " + entity.CurrentHP;
		damageText.text = "Damage - " + entity.Damage;
	}

	public void SetHP(float hp)
	{
		hpSlider.value = hp;
		hpText.text = "HP - " + hp.ToString();
	}

	public void SetMana(float mana)
	{
		manaSlider.value = mana;
		manaText.text = "Energy - " + mana.ToString();
	}
	
	public void SetDodje(float dodje)
	{
		dodgeText.text = "Dodge chance - " + dodje.ToString();
		
	}
	
	public void SetDamage(float damage)
	{
		dodgeText.text = "Dodge chance - " + damage.ToString();
		
	}
	
}
