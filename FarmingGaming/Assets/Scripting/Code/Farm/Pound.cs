using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pound : Building
{
    [SerializeField] private Creature[] _creaturesLib;
    public override int MaxUpgradeLevel => 1;
    private Creature _storedCreature;
    private DateTime _lastFeedingTime;
    private bool _isHungry;
    public bool IsHungry
    {
        get 
        {
            _isHungry = DateTime.Now.Subtract(_lastFeedingTime).Hours > 0;
            return _isHungry;
        }
    }

    private void Awake()
    {
        LoadSave();   
    }

    private void LoadSave()
    {
        int storedCreatureID = PlayerPrefs.GetInt(string.Format("Pound-{0}-creatureID", transform.position.ToString().GetHashCode()), -1);
        if (storedCreatureID < 0)
        {
        }
        else
        {
            _storedCreature = _creaturesLib.First((f) => f.ItemID == storedCreatureID);
            _lastFeedingTime = DateTime.Parse(PlayerPrefs.GetString(string.Format("Pound-{0}-lastFeeding", transform.position.ToString().GetHashCode())));
            _isHungry = DateTime.Now.Subtract(_lastFeedingTime).Hours > 0;
        }
    }

    public void PutCreatureIntoThePound(Creature creature)
    {
        _storedCreature = creature;
        //draw creature model in the pound
    }

    public void Feed()
    {
        Inventory inventory = Inventory.Instance;
        //use food from inventory
        if (inventory.Contain(_storedCreature.PrefferedFood, _storedCreature.RequiredFoodAmountToFeed))
        {
            inventory.Remove(_storedCreature.PrefferedFood, _storedCreature.RequiredFoodAmountToFeed);
            _lastFeedingTime = DateTime.Now;
            PlayerPrefs.SetString(string.Format("Pound-{0}-lastFeeding", transform.position.ToString().GetHashCode()), _lastFeedingTime.ToString());
            _isHungry = false;
        }
        else
            Debug.Log("Not enough food to feed the creature.");
    }
}
