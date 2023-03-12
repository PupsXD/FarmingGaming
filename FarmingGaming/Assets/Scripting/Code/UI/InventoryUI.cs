using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    
    [SerializeField] private InventorySlot[] _inventorySlots;
    private int _capacity = 20;
    
    public InventorySlot[]  InventorySlots
    {
        get => _inventorySlots;
        set => _inventorySlots = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _capacity = Inventory.Capacity;
        
        // not working while inventory UI disabled
        /*_inventorySlots = new InventorySlot[_capacity];
        
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();
        Debug.Log("Slot Count: " + slots.Length.ToString());
        if (slots.Length == _inventorySlots.Length)
        {
            _inventorySlots = slots;
        }*/

        /*for (int i = 0; i < _capacity; i++)
        {
            if (InventorySlots[i])
            {
                InventorySlots[i].OnItemRemove += RemoveItem;
            }
        }*/

        Inventory.Instance.OnSlotUpdated.AddListener(UpdateSlotUI);

    }

    public void UpdateSlotUI(int slot)
    {
        Inventory.Instance.GetSlotInfo(slot, out Item item, out int itemCount);
        if (item)
        {
           
            InventorySlots[slot].Item = item;
            InventorySlots[slot].Icon.sprite = item.Icon;
            InventorySlots[slot].Count = itemCount;
            InventorySlots[slot].CountText.text = itemCount.ToString();
        }
        else
        {
            InventorySlots[slot].Item = null;
            InventorySlots[slot].Icon.sprite = null;
            InventorySlots[slot].Count = 0;
            InventorySlots[slot].CountText.text = "";
        }
    }
    
    public void UpdateUI()
    {
        for (int i = 0; i < _capacity; i++)
        {
            if(!InventorySlots[i])
                continue;
            
           
            Inventory.Instance.GetSlotInfo(i, out Item item, out int itemCount);
            if (item)
            {
                InventorySlots[i].Item = item;
                InventorySlots[i].Icon.sprite = item.Icon;
                InventorySlots[i].Count = itemCount;
                InventorySlots[i].CountText.text = itemCount.ToString();
            }
            else
            {
                InventorySlots[i].Item = null;
                InventorySlots[i].Icon.sprite = null;
                InventorySlots[i].Count = 0;
                InventorySlots[i].CountText.text = "";
            }
            
        }
    }

    public void RemoveItem(int slot)
    {

        if(!InventorySlots[slot])
            return;
        if (InventorySlots[slot].Item)
        {
            Inventory.Instance.RemoveFromSlot(slot,InventorySlots[slot].Count);
            UpdateUI();
        }
    }

    public void ClearInventory()
    {
        Inventory.Instance.ClearInventory();
    }
}
