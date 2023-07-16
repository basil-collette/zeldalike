using Assets.Database.Model.Design;
using UnityEngine.EventSystems;

public class WeaponSlot : InventorySlot
{
    public sealed override void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggedItem.Item is Weapon)
        {
            DropProcess(draggedItem);
        }
    }

    public sealed override void Remove(Item item)
    {
        ObjectGetterHelper.Inventory.Weapon = null;
        FindGameObjectHelper.FindByName("Player").GetComponent<Player>().UnequipWeapon();
    }

    public sealed override void Add(Item item)
    {
        Weapon weapon = item as Weapon;
        ObjectGetterHelper.Inventory.Weapon = weapon;
        FindGameObjectHelper.FindByName("Player").GetComponent<Player>().EquipWeapon(weapon);
    }

}