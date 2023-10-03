using Assets.Database.Model.Design;
using UnityEngine.EventSystems;

public class HotbarSlot : InventorySlot
{
    public sealed override void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggedItem.Item is HoldableItem)
        {
            DropProcess(draggedItem);

            //remove from precedent list then add to the new one
        }
    }

    public sealed override void Remove(Item item)
    {
        MainGameManager._inventoryManager._hotbars.Remove(item as HoldableItem);
    }

    public sealed override void Add(Item item)
    {
        MainGameManager._inventoryManager._hotbars.Add(item as HoldableItem);
    }

}