using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pound : Building
{
    [SerializeField] private ItemsSubLib _creaturesLib;
    public override int MaxUpgradeLevel => 1;
    private Creature _storedCreature;
    private DateTime _lastFeedingTime;
    private bool _isHungry;

    private GameObject _creatureModel;

    public bool Empty
    {
        get { return _storedCreature == null; }
    }
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
        int storedCreatureID = PlayerPrefs.GetInt(string.Format("Pound-{0}-creatureID", transform.position.ToString().GetHashCode()), 0);
        if (storedCreatureID == 0)
        {
            //empty pound
        }
        else
        {
            _storedCreature = _creaturesLib.GetByID(storedCreatureID) as Creature;
            _lastFeedingTime = DateTime.Parse(PlayerPrefs.GetString(string.Format("Pound-{0}-lastFeeding", transform.position.ToString().GetHashCode())));
            _isHungry = DateTime.Now.Subtract(_lastFeedingTime).Hours > 0;

            _creatureModel = Instantiate(_storedCreature.WorldCreatureModel, transform);
            StartCoroutine(CreatureModelDummyAnmator());
        }
    }

    public void PutCreatureIntoThePound(Creature creature)
    {
        if (!Empty) return;
        _storedCreature = creature;
        DateTime now = DateTime.Now;
        _lastFeedingTime = new DateTime(now.Year,now.Month,now.Day);
        PlayerPrefs.SetInt(string.Format("Pound-{0}-creatureID", transform.position.ToString().GetHashCode()), _storedCreature.ItemID);
        PlayerPrefs.SetString(string.Format("Pound-{0}-lastFeeding", transform.position.ToString().GetHashCode()), _lastFeedingTime.ToString());

        _creatureModel = Instantiate(_storedCreature.WorldCreatureModel, transform);
        StartCoroutine(CreatureModelDummyAnmator());
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

    IEnumerator CreatureModelDummyAnmator()
    {
        Vector2 s, f;
        while(_creatureModel != null)
        {
            s = _creatureModel.transform.position;
            f = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle;
            for (float t = 0; t <= 1;)
            {
                t += Time.deltaTime;
                _creatureModel.transform.position = Vector2.Lerp(s, f, t);
                yield return null;
            }
            yield return new WaitForSeconds(1);
        }
    }
    [ContextMenu("Add Random Creature To Inventory")]
    private void AddRandomCreatureToInventory()
    {
        Inventory.Instance.TryAdd(_creaturesLib.GetRandom());
    }
    [ContextMenu("Add Random Creature To Pound")]
    private void AddRandomCreatureToPound()
    {
        PutCreatureIntoThePound(_creaturesLib.GetRandom() as Creature);
    }
}
