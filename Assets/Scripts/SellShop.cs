using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using TMPro;

public class SellShop : MonoBehaviour
{
    public static event Action<bool> OnCloseSellShop;
    public static event Action<InventoryItemPrefab> OnitemSold;
    
    #region Exposed_Variables
    
    [Header("Sell Shop References")]
    [SerializeField] private InventoryItemPrefab itemPrefab;
    [SerializeField] private Transform sellItemsContainer;
    [SerializeField] private TextMeshProUGUI sellingNameText;
    [SerializeField] private TextMeshProUGUI sellingPriceText;


    #endregion

    #region Private_Variables
    
    private List<PlayerBodyEquipment> _equipableSlots = new List<PlayerBodyEquipment>();
    private List<InventoryItemPrefab> _sellingList = new List<InventoryItemPrefab>();
    private List<InventoryItemPrefab> _actualItemLs = new List<InventoryItemPrefab>();
    private List<InventoryItemPrefab> _inventoryItems = new List<InventoryItemPrefab>();
    private int _currentItemSellingPrice;
    private InventoryItemPrefab _itemToBeSold = null;
    #endregion

    #region Public_Variables

    #endregion

    #region Unity_Calls
    
    private void OnEnable()
    {
        if (_equipableSlots.Count < 1)
            _equipableSlots = GameManager.Instance.GetBodyEquipment();
        
        _inventoryItems = UIManager.Instance.Inventory.InventoryItems;
        PopulateSellShop();
        sellingNameText.text = $"Select Item";
        sellingPriceText.text = $"000 Coins";
    }
    
    private void Update()
    {
        if (_sellingList.Count != 0) return;
        sellingNameText.text = $"Select Item";
        sellingPriceText.text = $"000 Coins";
    }

    #endregion

    #region Private_Methods
    
    private void PopulateSellShop()
    {
        if (_inventoryItems.Count < 1) return;
        foreach (var item in _inventoryItems)
        {
            foreach (var sellItem in _sellingList.Where(sellItem => sellItem.ItemId == item.ItemId))
            {
                sellItem.UpdateQuantity(item.Quantity);
                sellItem.SetData(item.CurrentData);
                sellItem.itemButton.onClick.AddListener(() => OnSellClick(sellItem));
                sellItem.Equipped = item.Equipped;
            }
            
            if (!_actualItemLs.Contains(item))
            {
                var newItem = Instantiate(item, sellItemsContainer);
                _sellingList.Add(newItem);
                _actualItemLs.Add(item);
                newItem.UpdateQuantity(item.Quantity);
                newItem.SetData(item.CurrentData);
                newItem.Equipped = item.Equipped;
                newItem.itemButton.onClick.AddListener(() => OnSellClick(newItem));
            }
        }
    }

    private void OnSellClick(InventoryItemPrefab item)
    {
        _currentItemSellingPrice = item.CurrentData.itemPrice/2;
        sellingNameText.text = item.CurrentData.itemName;
        sellingPriceText.text = $"{_currentItemSellingPrice} Coins";
        _itemToBeSold = item;
    }

    #endregion

    #region Public_Methods
    
    public void SellItem()
    {
        if(_itemToBeSold == null) return;
        
        switch (_itemToBeSold.Quantity)
        {
            case > 1:
            {
                CoinManager.Instance.AddCoins(_currentItemSellingPrice);
                _itemToBeSold.SetItemQuantity(false);
                OnitemSold?.Invoke(_itemToBeSold);
                break;
            }
            case 1:
            {
                if (_itemToBeSold.Equipped)
                {
                    UIManager.Instance.DisplayError($"{_itemToBeSold.nameText.text} is equipped, it cannot be sold.");
                    break;
                }

                CoinManager.Instance.AddCoins(_currentItemSellingPrice);
                _itemToBeSold.SetItemQuantity(false);
                OnitemSold?.Invoke(_itemToBeSold);
                break;
            }
            case < 1:
            {
                InventoryItemPrefab refItem = null;
                foreach (var item in _actualItemLs)
                {
                    if (_sellingList.Any(sellItem => sellItem.ItemId == item.ItemId))
                    {
                        refItem = item;
                    }
                }

                _actualItemLs.Remove(refItem);
                _sellingList.Remove(_itemToBeSold);
                Destroy(_itemToBeSold.gameObject);
                _itemToBeSold = null;
                break;
            }
        }
    }

    public void CloseShop()
    {
        OnCloseSellShop?.Invoke(true);
        gameObject.SetActive(false);
    }

    #endregion

    
}
