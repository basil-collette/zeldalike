using Assets.Database.Model.Design;
using Assets.Scripts.Enums;
using UnityEngine.EventSystems;

public class WeaponSlot : InventorySlot
{
    public sealed override void Remove(Item item)
    {
        MainGameManager._inventoryManager._weapon = null;
        FindAnyObjectByType<Player>().UnequipWeapon();
    }

    public sealed override void Add(Item item)
    {
        Weapon weapon = item as Weapon;
        MainGameManager._inventoryManager._weapon = weapon;
        FindAnyObjectByType<Player>().EquipWeapon(weapon);
    }

    public sealed override bool CanBeDraggedHere(DraggableItem draggedItem)
    {
        return draggedItem.Item.ItemType == ItemTypeEnum.weapon;
    }

}