using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Pound : Building, IItemConsumer
{
    [SerializeField] private ItemsSubLib _creaturesLib;
    [SerializeField] private HungryMarker _hungryMarker;
    [HideInInspector] public UnityEvent OnBeingFeeded = new UnityEvent();
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
            Instantiate(_hungryMarker, _creatureModel.transform).Connect(this);
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
        Instantiate(_hungryMarker, _creatureModel.transform).Connect(this);
        StartCoroutine(CreatureModelDummyAnmator());
    }

    public void Feed(int startFromSpecificInventorySlot = -1)
    {
        Inventory inventory = Inventory.Instance;
        //use food from inventory
        if (inventory.Contain(_storedCreature.PrefferedFood, _storedCreature.RequiredFoodAmountToFeed))
        {
            if(startFromSpecificInventorySlot < 0)
                inventory.Remove(_storedCreature.PrefferedFood, _storedCreature.RequiredFoodAmountToFeed);
            else
            {
                inventory.GetSlotInfo(startFromSpecificInventorySlot, out Item item, out int itemsInSlot);
                if(itemsInSlot < _storedCreature.RequiredFoodAmountToFeed)
                {
                    int additionalAmountToRemove = _storedCreature.RequiredFoodAmountToFeed - itemsInSlot;
                    inventory.RemoveFromSlot(startFromSpecificInventorySlot, itemsInSlot);
                    inventory.Remove(item, additionalAmountToRemove);
                }
                else
                {
                    inventory.RemoveFromSlot(startFromSpecificInventorySlot, _storedCreature.RequiredFoodAmountToFeed);
                }
            }
            _lastFeedingTime = DateTime.Now;
            PlayerPrefs.SetString(string.Format("Pound-{0}-lastFeeding", transform.position.ToString().GetHashCode()), _lastFeedingTime.ToString());
            _isHungry = false;
            OnBeingFeeded.Invoke();
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

    public bool CanConsume(Item item)
    {
        if (item is Creature)
            return Empty;
        if (item is Food && _storedCreature != null)
            return _storedCreature.PrefferedFood.ItemID == item.ItemID;
        return false;
    }

    public void Consume(int sourceInventorySlot)
    {
        Inventory.Instance.GetSlotInfo(sourceInventorySlot, out Item item, out int storedAmount);
        if(item is Creature && storedAmount > 0 && Empty)
        {
            Inventory.Instance.RemoveFromSlot(sourceInventorySlot, 1);
            PutCreatureIntoThePound(item as Creature);
        }
        if(item is Food && storedAmount > 0 && !Empty && _storedCreature.PrefferedFood.ItemID == item.ItemID)
        {
            if (_isHungry)
                Feed(sourceInventorySlot);
            else
                Debug.Log("Creature already feeded.");
        }
    }
}
