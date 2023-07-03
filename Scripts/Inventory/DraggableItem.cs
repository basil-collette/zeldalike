using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Database.Model.Design;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [SerializeReference] public Item Item;
    [HideInInspector] public Transform slot;

    public void OnBeginDrag(PointerEventData eventData)
    {
        slot = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 lastMousePosition = Input.mousePosition;
        Vector2? lastTouchPosition = null;

        if (Input.touchCount > 0)
            lastTouchPosition = Input.touches[0].position;

        transform.position = (Vector3)((lastTouchPosition != null) ? lastTouchPosition : lastMousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(slot);
        image.raycastTarget = true;
    }

}