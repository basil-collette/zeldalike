using Assets.Database.Model.Design;
using Assets.Scripts.Items.Equipments.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : InventorySlot, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggedItem.Item is Weapon)
        {
            DropProcess(draggedItem);
        }
    }

}