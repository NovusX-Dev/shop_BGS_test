using UnityEngine;

public class PlayerBodyEquipment : MonoBehaviour
{
    #region Exposed_Variables
    
    [SerializeField] private Sprite nakedEquipment;
    [SerializeField] private GlobalEnums.EquipmentType slotType;
    [SerializeField] private bool centerEquipment;
    [SerializeField] private bool leftEquipment;
    [SerializeField] private bool rightEquipment;

    #endregion

    #region Private_Variables

    private bool _isNaked = true;
    private SpriteRenderer _spriteRenderer;
    private ItemSo _equippedItemSo = null;

    #endregion

    #region Public_Variables

    public GlobalEnums.EquipmentType SlotType => slotType;

    #endregion

    #region Unity_Calls

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #endregion

    #region Private_Methods

    

    #endregion

    #region Public_Methods

    public void EquipItem(ItemSo itemToBeEquiped, ref bool equipped)
    {
        if ((int) itemToBeEquiped.itemType != (int) slotType)
        {
            equipped = false;
            return;
        }
        if(centerEquipment)
        {
            _spriteRenderer.sprite = itemToBeEquiped.centerSprite;
            _isNaked = false;
        }
        else if(leftEquipment)
        {
            _spriteRenderer.sprite = itemToBeEquiped.leftSprite;
            _isNaked = false;
        }
        else if(rightEquipment)
        {
            _spriteRenderer.sprite = itemToBeEquiped.rightSprite;
            _isNaked = false;
        }

        equipped = true;
        _equippedItemSo = itemToBeEquiped;
    }

    public void UnEquipItem()
    {
        _equippedItemSo = null;
        _spriteRenderer.sprite = nakedEquipment;
        _isNaked = true;
    }

    #endregion


}
