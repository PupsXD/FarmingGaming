using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    
    [SerializeField] private InventorySlot[] _inventorySlots;
    [SerializeField] private Image _draggedItem;
    [SerializeField] private LayerMask _whatIsItemsConsumer;
    private int _capacity = 20;
    private bool _isDraggingItem;
    private int _draggingFromSlotNum;
    private Item _draggedItemReference;

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

        //Inventory.Instance.OnSlotUpdated.AddListener(UpdateSlotUI);

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

    public void StartDragging(int fromSlotNum)
    {
        Inventory.Instance.GetSlotInfo(fromSlotNum, out _draggedItemReference, out int itemsAmoun);
        if (_draggedItemReference == null) return;
        _draggedItem.sprite = _draggedItemReference.Icon;
        _draggingFromSlotNum = fromSlotNum;
        _draggedItem.gameObject.SetActive(true);
        _isDraggingItem = true;
    }
    private void CancelDragging()
    {
        _isDraggingItem = false;
        _draggedItem.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (_isDraggingItem)
        {
            _draggedItem.transform.position = Input.mousePosition;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, _whatIsItemsConsumer);

            if (hit.collider != null)
            {
                if (hit.transform.TryGetComponent(out IItemConsumer itemConsumer))
                {
                    if (itemConsumer.CanConsume(_draggedItemReference))
                    {
                        _draggedItem.color = new Color(1, 1, 1, 145f / 256f); ;

                        if (Input.GetMouseButtonUp(0))
                        {
                            itemConsumer.Consume(_draggingFromSlotNum);
                            CancelDragging();
                        }
                    }
                }
                else
                    _draggedItem.color = new Color(1, 0, 0, 145f / 256f);
            }
            else
                _draggedItem.color = new Color(1, 0, 0, 145f / 256f);
            if (Input.GetMouseButtonUp(0))
            {
                CancelDragging();
            }
        }
    }
}
