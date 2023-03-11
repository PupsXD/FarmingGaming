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

	public void SetHUD(CombatEntity entity)
	{
		nameText.text = entity.Name;
		levelText.text = "Lvl " + entity.Level;
		hpSlider.maxValue = entity.MaxHP;
		hpSlider.value = entity.MaxHP;
		manaSlider.maxValue = entity.MaxMana;
		manaSlider.value = 0;
	}

	public void SetHP(float hp)
	{
		hpSlider.value = hp;
	}

	public void SetMana(float mana)
	{
		manaSlider.value = mana;
	}
}
