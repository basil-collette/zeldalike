using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Database.Model.Design;
using Assets.Scripts.Enums;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image Background;
    public Image Shadow;
    public Image Image;
    [SerializeReference] public Item Item;
    [HideInInspector] public Transform Slot;

    public void SetItem(Item item)
    {
        Item = item;

        (Color, Color) colors = GetColors();
        Background.color = colors.Item1;
        Shadow.color = colors.Item2;
    } 

    (Color, Color) GetColors()
    {
        switch (Item.ItemType)
        {
            case ItemTypeEnum.weapon:
                return (new Color(1, 1, 1, 0.353f), new Color(0, 0, 0, 0.815f));

            case ItemTypeEnum.holdable:
                return (new Color(1, 0.125f, 0, 0.254f), new Color(1, 1, 1, 0.603f));

            //case consommable ? vert, bleu ?

            case ItemTypeEnum.item:
            default:
                return (new Color(0, 0, 0, 0), new Color(0, 0, 0, 0));
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Slot = transform.parent;

        Slot.GetComponent<InventorySlot>().Remove(Item);

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Background.raycastTarget = false;

        Background.color = new Color(0, 0, 0, 0);
        Shadow.color = new Color(0, 0, 0, 0);

        if (Item.ItemType == ItemTypeEnum.weapon)
        {
            //indicate free and coresponding slots
        }

        if (Item.ItemType == ItemTypeEnum.holdable)
        {
            //indicate free and coresponding slots
        }
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
        transform.SetParent(Slot);
        Background.raycastTarget = true;

        (Color, Color) colors = GetColors();
        Background.color = colors.Item1;
        Shadow.color = colors.Item2;
    }

}