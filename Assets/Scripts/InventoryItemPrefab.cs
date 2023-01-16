using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemPrefab : MonoBehaviour
{
    #region Exposed_Variables

    [field: SerializeField] public TextMeshProUGUI nameText = null;
    [field: SerializeField] public TextMeshProUGUI priceText = null;
    [field: SerializeField] public TextMeshProUGUI quantityText = null;
    [field: SerializeField] public Image itemImage = null;
    [field: SerializeField] public Button itemButton = null;

    #endregion

    #region Private_Variables

    private int _quantity = 0;
    private ItemSo _currentData = null;
    private GlobalEnums.EquipmentType _equipmentType = GlobalEnums.EquipmentType.Invalid;

    #endregion

    #region Public_Variables

    public ItemSo CurrentData => _currentData;
    public PlayerBodyEquipment CurrentBodyEquipment { get; private set; }
    public int Quantity => _quantity;
    public  bool Equipped { get; set; }
    public  int ItemId {get; private set;}

    #endregion

    #region Unity_Calls

    private void OnEnable()
    {
        quantityText.gameObject.SetActive(_quantity > 1);
    }

    #endregion

    #region Private_Methods

    #endregion

    #region Public_Methods

    public void SetItemQuantity(bool add)
    {
        if (add) _quantity++;
        else _quantity--;
        quantityText.text = _quantity.ToString();
        quantityText.gameObject.SetActive(_quantity > 1);
        if (_quantity < 0) _quantity = 0;
    }

    public void SetBodyEquipment(PlayerBodyEquipment body)
    {
        CurrentBodyEquipment = body;
        _equipmentType = body == null ? GlobalEnums.EquipmentType.Invalid : body.SlotType;
    }

    public void UpdateQuantity(int amount)
    {
        _quantity = amount;
        quantityText.text = _quantity.ToString();
        quantityText.gameObject.SetActive(_quantity > 1);
    }

    public void SetData(ItemSo data)
    {
        _currentData = data;
        ItemId = data.itemId;
    }

    #endregion


}