using UnityEngine.EventSystems;

public class HotbarSlot : InventorySlot
{
    public new void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggedItem.Item is HoldableItem)
        {
            DropProcess(draggedItem);
        }
    }

}