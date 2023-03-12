using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private int _slotNum;
    [SerializeField] private InventoryUI _ui;
    [SerializeField] private CustomButton _itemButton;
    private int _count = 0;
    
    private Item _item;
    public Item Item { get => _item; set => _item = value; }
    public Image Icon { get => _icon; set => _icon = value; }
    public int Count { get => _count; set => _count = value; }
    public TextMeshProUGUI CountText { get => _countText; set => _countText = value; }

    public delegate void IntDelegate(int value);
    public event IntDelegate OnItemRemove;

    private void Awake()
    {
        _itemButton.PointerDown.AddListener(() =>_ui.StartDragging(_slotNum));
    }

    private void OnEnable()
    {
        if (!_ui)
            _ui = FindObjectOfType<InventoryUI>();
    }

    public void ItemRemove()
    {
        Debug.Log("Item Remove button");
        if(_ui)
            _ui.RemoveItem(_slotNum);
        //OnItemRemove(_slotNum);
    }
    
}
