using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class Shop : MonoBehaviour
{
    public static event Action<bool> OnCloseBuyShop;


    #region Exposed_Variables

    [Header("List of items in the shop")]
    [SerializeField] private ItemGroupSo shopItemSos;

    [Header("Buy Shop References")] 
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private InventoryItemPrefab itemPrefab;
    [SerializeField] private TextMeshProUGUI buyingText;
    [SerializeField] private TextMeshProUGUI itemToBuyNameText = null;
    [SerializeField] private TextMeshProUGUI itemToBuyCostText = null;
    
    #endregion

    #region Private_Variables
    
    private List<InventoryItemPrefab> _currentItemsList = null;
    private ItemSo _itemToBuy = null;
    private List<PlayerBodyEquipment> _playerEquipSlots = new List<PlayerBodyEquipment>();
    private PlayerInventory _playerInventory = null;

    #endregion

    #region Public_Variables

    #endregion

    #region Unity_Calls
        
    private void OnEnable()
    {
        PopulateBuyCategory("HeadWear");
        if (_playerEquipSlots.Count < 1)
            _playerEquipSlots = GameManager.Instance.GetBodyEquipment();
    }

    private void Start()
    {
        _playerInventory = UIManager.Instance.Inventory;
    }

    #endregion

    #region Private_Methods
    
    private void OnSetItemToBuy(ItemSo item)
    {
        if (item == null)
        {
            itemToBuyNameText.text = $"Select Item";
            itemToBuyCostText.text = $"000 Coins";
            return;
        }
        _itemToBuy = item;
        itemToBuyNameText.text = _itemToBuy.itemName;
        itemToBuyCostText.text = $"{_itemToBuy.itemPrice.ToString()} Coins";
        Debug.Log($"Item to buy is {_itemToBuy.itemName}");
    }
    #endregion

    #region Public_Methods
    
    public void PopulateBuyCategory(string itemType)
    {
        if (_currentItemsList != null)
        {
            foreach (var item in _currentItemsList)
            {
                Destroy(item.gameObject);
            }
        }
        OnSetItemToBuy(null);
        var newtItemsList = new List<InventoryItemPrefab>();

        for (int i = 0; i < shopItemSos.ItemSos.Length; i++)
        {
            var newItem = shopItemSos.ItemSos[i];

            //populate items
            if (shopItemSos.ItemSos[i].itemType.ToString() == itemType)
            {
                var itemObject = Instantiate(itemPrefab, itemsContainer);
                newtItemsList.Add(itemObject);
                itemObject.nameText.text = newItem.itemName;
                itemObject.priceText.text = $"{newItem.itemPrice} Coins";
                if (newItem.centerSprite != null)
                {
                    itemObject.itemImage.sprite = newItem.centerSprite;
                }
                else if (newItem.leftSprite != null)
                {
                    itemObject.itemImage.sprite = newItem.leftSprite;
                }
                
                itemObject.itemButton.onClick.AddListener(() => OnSetItemToBuy(newItem));
            }
        }
        _currentItemsList = newtItemsList;
    }
    
    //Button
    public void OnBuyClick()
    {
        if (_itemToBuy == null)
        {
            StartCoroutine(DisplayBoughtText($"Select an item to buy."));
            return;
        }
        if (CoinManager.Instance.GetCoinAmount() >= _itemToBuy.itemPrice)
        {
            CoinManager.Instance.DeductCoins(_itemToBuy.itemPrice);
            _playerInventory.AddItem(_itemToBuy);
            StartCoroutine(DisplayBoughtText($"You bought {_itemToBuy.itemName} for: {_itemToBuy.itemPrice} Coins"));
        }
        else
        {
            StartCoroutine(DisplayBoughtText("Not Enough Coins!"));
        }
        
        IEnumerator DisplayBoughtText(string text)
        {
            buyingText.text = text;
            yield return new WaitForSeconds(2f);
            buyingText.text = "";
        }
    }
    
    public void CloseShop()
    {
        OnCloseBuyShop?.Invoke(true);
        gameObject.SetActive(false);
    }
    
    #endregion
   

    
    

    

    
    
    


    
}
