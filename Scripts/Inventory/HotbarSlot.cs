using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot : InventorySlot, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggedItem.Item is HoldableItem)
        {
            DropProcess(draggedItem);
        }
    }

}