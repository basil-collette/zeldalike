using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (transform.childCount != 0)
        {
            Transform currentChild = transform.GetChild(0);
            currentChild.SetParent(draggedItem.slot);
        }
        
        draggedItem.slot = transform;
    }

}