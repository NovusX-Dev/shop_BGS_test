using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static event Action<bool> OnInventoryClosed;
    
    #region Exposed_Variables

    [SerializeField] private GameObject mainPanel = null;
    [SerializeField] private Transform itemsContainer = null;
    [SerializeField] private InventoryItemPrefab itemPrefab = null;
    [SerializeField] private TextMeshProUGUI selectedNameText = null;
    [SerializeField] private Button unEquipButton = null;

    #endregion

    #region Private_Variables

    private List<PlayerBodyEquipment> _playerEquipSlots = new List<PlayerBodyEquipment>();
    private List<InventoryItemPrefab> _inventoryItems = new List<InventoryItemPrefab>();
    private InventoryItemPrefab _currentlySelectedItem = null;

    #endregion

    #region Public_Variables

    public List<InventoryItemPrefab> InventoryItems => _inventoryItems;
    public GameObject MainPanel => mainPanel;
    #endregion

    #region Unity_Calls
    
    private void OnEnable()
    {
        SellShop.OnitemSold += UpdateSoldItem;
        PlayerController.OnGetBodyEquipment += SetBodyEquipment;
    }

    private void OnDisable()
    {
        SellShop.OnitemSold -= UpdateSoldItem;
        PlayerController.OnGetBodyEquipment -= SetBodyEquipment;
    }

    private void Update()
    {
        if (_currentlySelectedItem != null) return;
        unEquipButton.interactable = false;
    }

    #endregion

    #region Private_Methods

    private void SetBodyEquipment(List<PlayerBodyEquipment> equipments)
    {
        _playerEquipSlots = equipments;
    }
    
    private void SelectItem(InventoryItemPrefab item)
    {
        _currentlySelectedItem = item;
        selectedNameText.text = _currentlySelectedItem.nameText.text;
        unEquipButton.interactable = true;
    }

    private void UpdateSoldItem(InventoryItemPrefab item)
    {
        InventoryItemPrefab itemRef = null;
        foreach (var invItem in _inventoryItems.Where(invItem => invItem.ItemId == item.ItemId))
        {
            invItem.UpdateQuantity(item.Quantity);
            itemRef = invItem;
            break;
        }

        if (itemRef == null) return;
        if (itemRef.Quantity < 1)
        {
            _inventoryItems.Remove(itemRef);
            Destroy(itemRef.gameObject);
        }
    }

    #endregion

    #region Public_Methods

    public void AddItem(ItemSo data)
    {
        if(_inventoryItems.Count > 0)
        {
            foreach (var item in _inventoryItems.Where(item => item.ItemId == data.itemId))
            {
                item.SetItemQuantity(true);
                return;
            }
        }
        var itemToAdd = Instantiate(itemPrefab, itemsContainer);
        itemToAdd.nameText.text = data.itemName;
        itemToAdd.name = data.itemName;
        itemToAdd.priceText.gameObject.SetActive(false);
        itemToAdd.itemImage.sprite = data.centerSprite;
        itemToAdd.itemButton.onClick.AddListener(() => SelectItem(itemToAdd));
        itemToAdd.SetData(data);
        itemToAdd.SetItemQuantity(true);
        if (data.centerSprite != null)
        {
            itemToAdd.itemImage.sprite = data.centerSprite;
        }
        else if (data.leftSprite != null)
        {
            itemToAdd.itemImage.sprite = data.leftSprite;
        }
        
        _inventoryItems.Add(itemToAdd);
    }

    public void EquipItem()
    {
        var equipped = false; 
        foreach(var slot in _playerEquipSlots)
        {
            slot.EquipItem(_currentlySelectedItem.CurrentData, ref equipped);
            if (equipped)
            {
                _currentlySelectedItem.Equipped = true;
                _currentlySelectedItem.SetBodyEquipment(slot);
                CloseShop();
                break;
            }
        }
    }

    public void UnEquipItem()
    {
        if (_currentlySelectedItem == null || !_currentlySelectedItem.Equipped) return;
        _currentlySelectedItem.Equipped = false;
        _currentlySelectedItem.CurrentBodyEquipment.UnEquipItem();
        _currentlySelectedItem.SetBodyEquipment(null);
        CloseShop();
    }
    
    public void CloseShop()
    {
        OnInventoryClosed?.Invoke(true);
        mainPanel.SetActive(false);
    }

    #endregion


}