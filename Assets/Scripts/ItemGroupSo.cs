using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Group", order = 0)]
public class ItemGroupSo : ScriptableObject
{
    [SerializeField] private ItemSo[] itemSos;

    public ItemSo[] ItemSos => itemSos;
}