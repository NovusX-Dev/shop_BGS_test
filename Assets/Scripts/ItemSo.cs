using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemSo : ScriptableObject
{
    public enum Type { HeadWear, ChestWear, LowerChest, FootWear, Belts}
    public Type itemType;

    public int itemId;
    public string itemName;
    public int itemPrice;
    public Sprite centerSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
}
