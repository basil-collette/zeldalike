using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        DropProcess(draggedItem);
    }

    protected void DropProcess(DraggableItem draggedItem)
    {
        if (transform.childCount != 0)
        {
            DraggableItem currentItem = transform.GetChild(0).GetComponent<DraggableItem>();
            currentItem.transform.SetParent(draggedItem.Slot);
            currentItem.Item.InventoryIndex = draggedItem.Slot.GetSiblingIndex();
        }

        draggedItem.Slot = transform;
        draggedItem.Item.InventoryIndex = transform.GetSiblingIndex();
    }

}