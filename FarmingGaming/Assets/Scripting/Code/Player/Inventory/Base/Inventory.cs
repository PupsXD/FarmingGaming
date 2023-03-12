using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    private static Inventory _instance;
    public const int Capacity = 20;
    public static Inventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Inventory>();
                _instance.Load();
            }
            return _instance;
        }
    }

    [SerializeField] private ItemsSubLib _allItems;

    private Item[] _itemList = new Item[Capacity];
    private int[] _amount = new int[Capacity];

    [HideInInspector] public UnityEvent<int> OnSlotUpdated = new UnityEvent<int>();

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            Load();
        }
        DebugInventoryContent();
    }

    public void UpdateUI()
    {
        InventoryUI ui = FindObjectOfType<InventoryUI>();
        if (ui)
        {
            ui.UpdateUI();
        }
    }
    
    
    public Inventory Load()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        int storedItemId, itemsInStack;

        for (int i = 0; i < Capacity; i++)
        {
            storedItemId = PlayerPrefs.GetInt(string.Format("Inventory-slot{0}-itemID", i), 0);
            if (storedItemId != 0)
            {
                itemsInStack = PlayerPrefs.GetInt(string.Format("Inventory-slot{0}-itemsCount", i), 0);
                var item = _allItems.GetByID(storedItemId);
                if (item != null)
                {
                    Debug.Log(i + ": " + item.ItemName);
                    inventory.ForceAddToSlot(i, item, itemsInStack);
                }
            }
        }
        UpdateUI();

        return inventory;
    }

    public bool TryAdd(Item item, int amount =1)
    {
        List<int> sameItems = new List<int>();
        List<int> emptySlots = new List<int>();
        int availableSpace = 0;
        Debug.Log("Try to add item to Inventory");
        for (int i = 0; i < Capacity; i++)
        {
            if (_itemList[i] != null)
            {
                if(_itemList[i].ItemID == item.ItemID && _amount[i] < item.CountPerStack)
                {
                    sameItems.Add(i);
                    availableSpace = item.CountPerStack - _amount[i];
                }
            }
            else
                emptySlots.Add(i);

        }
        availableSpace += emptySlots.Count * item.CountPerStack;
        bool result = availableSpace >= amount;

        if (result)
        {

            foreach (int i in sameItems) //fill not full stacks
            {
                int freeSpace = item.CountPerStack - _amount[i];
                int addedAmount = amount > freeSpace ? freeSpace : amount;
                _amount[i] += addedAmount;
                amount -= addedAmount;
                Debug.Log(string.Format("Slot[{0}]: Item type = {1}; Added amount = {2}; Total amount = {3};", i, _itemList[i].ItemName, addedAmount, _amount[i]));

                PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemID", i), item.ItemID);
                PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemsCount", i), _amount[i]);
                OnSlotUpdated.Invoke(i);
                if (amount == 0) break;
            }
            foreach (int i in emptySlots) //fill empty slots
            {
                _itemList[i] = item;
                int freeSpace = item.CountPerStack - _amount[i];
                int addedAmount = amount > freeSpace ? freeSpace : amount;
                _amount[i] += addedAmount;
                amount -= addedAmount;
                Debug.Log(string.Format("Slot[{0}]: Item type = {1}; Added amount = {2}; Total amount = {3};", i, _itemList[i].ItemName, addedAmount, _amount[i]));
                
                PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemID", i), item.ItemID);
                PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemsCount", i), _amount[i]);
                OnSlotUpdated.Invoke(i);
                if (amount == 0) break;
            }

        }

        UpdateUI();
        return result;
    }

    public void Remove(Item item, int amount = 1)
    {
        for (int i = 0; i < Capacity; i++)
        {
            if (_itemList[i] == null) continue;
            if (_itemList[i].ItemID == item.ItemID)
            {
                if (_amount[i] > amount)
                {
                    _amount[i] -= amount;
                    amount = 0;
                    PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemsCount", i), _amount[i]);
                    OnSlotUpdated.Invoke(i);
                    break;
                }
                else
                {
                    amount -= _amount[i];
                    _itemList[i] = null;
                    _amount[i] = 0;
                    PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemID", i), 0);
                    PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemsCount", i), 0);
                    OnSlotUpdated.Invoke(i);
                }
            }
        }
        if (amount > 0)
            Debug.LogError(string.Format("Not enough items of type {0} in the inventory to remove!", item.ItemName));
        UpdateUI();
    }

    public void ClearInventory()
    {
        for (int i = 0; i < Capacity; i++)
        {
            _amount[i] = 0;
            _itemList[i] = null;
            PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemID", i), 0);
            PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemsCount", i), 0);
        }
        UpdateUI();
    }
    public bool TryAddToSlot(int slot, Item item, int amount = 1)
    {
        if (_itemList[slot] == null) return false;
        if (_itemList[slot].ItemID == item.ItemID)
        {
            int freeSpace = item.CountPerStack - _amount[slot];
            if(freeSpace < amount)
            {
                int amountToAddToEmptySlots = amount - freeSpace;
                _amount[slot] += freeSpace;
                PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemsCount", slot), _amount[slot]);
                TryAdd(item, amountToAddToEmptySlots);
            }
            else
            {
                _amount[slot] += amount;
                PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemsCount", slot), _amount[slot]);
            }
            OnSlotUpdated.Invoke(slot);
            UpdateUI();
            return true;
        }
        return false;
    }
    private void ForceAddToSlot(int slot, Item item, int amount = 1)
    {
        _itemList[slot] = item;
        _amount[slot] = amount;
        PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemID", slot), item.ItemID);
        PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemsCount", slot), amount);
        OnSlotUpdated.Invoke(slot);
    }
    public void RemoveFromSlot(int slot, int amount = 1)
    {
        if (_itemList[slot] != null)
        {
            _amount[slot] -= amount;
            
            if (_amount[slot] <= 0)
            {
                _amount[slot] = 0;
                _itemList[slot] = null;
                PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemID", slot), 0);
                PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemsCount", slot), 0);
            }
            else
            {
                PlayerPrefs.SetInt(string.Format("Inventory-slot{0}-itemsCount", slot), _amount[slot]);
            }
            
            OnSlotUpdated.Invoke(slot);
                   
            UpdateUI();
        }
    }
    
    
    public bool Contain(Item item, int amount = 1)
    {
        int contain = 0;
        for (int i = 0; i < Capacity; i++)
        {
            if (_itemList[i] == null) continue;
            if (_itemList[i].ItemID == item.ItemID)
            {
                contain += _amount[i];
                if (contain >= amount)
                    return true;
            }
        }
        return false;
    }

    public void GetSlotInfo(int slotNum, out Item storedItem, out int storedItemsAmount)
    {
        storedItem = _itemList[slotNum];
        storedItemsAmount = _amount[slotNum];
    }
    
    public void DebugInventoryContent()
    {
        for (int i = 0; i < _itemList.Length; i++)
        {
            if (_itemList[i] == null)
                Debug.Log(string.Format("Slot[{0}]: Item type = None; Items amount = 0;", i));
            else
                Debug.Log(string.Format("Slot[{0}]: Item type = {1}; Items amount = {2};", i, _itemList[i].ItemName, _amount[i]));
        }
    }
}