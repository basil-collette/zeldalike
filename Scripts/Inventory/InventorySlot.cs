using Assets.Database.Model.Design;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public virtual void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        DropProcess(draggedItem);
    }

    protected void DropProcess(DraggableItem draggedItem)
    {
        if (transform.childCount != 0)
        {
            DraggableItem currentItem = transform.GetChild(0).GetComponent<DraggableItem>();

            Remove(currentItem.Item);
            draggedItem.Slot.GetComponent<InventorySlot>().Add(currentItem.Item);

            currentItem.transform.SetParent(draggedItem.Slot);
            currentItem.Item.InventoryIndex = draggedItem.Slot.GetSiblingIndex();
        }

        draggedItem.Slot = transform;
        draggedItem.Item.InventoryIndex = transform.GetSiblingIndex();

        Add(draggedItem.Item);
    }

    public virtual void Remove(Item item)
    {
        MainGameManager._inventoryManager._items.Remove(item);
    }

    public virtual void Add(Item item)
    {
        MainGameManager._inventoryManager._items.Add(item);
    }

}