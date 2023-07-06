using Assets.Database.Model.Design;
using UnityEngine.EventSystems;

public class WeaponSlot : InventorySlot
{
    public new void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggedItem.Item is Weapon)
        {
            DropProcess(draggedItem);
        }
    }

}