using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public DraggableItem draggableItem;

    Inventory Inventory;

    private void OnEnable()
    {
        Inventory = FindGameObjectHelper.FindByName("Player").GetComponent<Player>().inventory;

        FillSlots();
    }

    void FillSlots()
    {
        DraggableItem[] slots = transform.Find("InventoryBox").GetComponentsInChildren<DraggableItem>();
        for (int i = 0; i < Mathf.Min(Inventory.items.Count, slots.Length); i++)
        {
            DraggableItem item = Instantiate(draggableItem, transform.position, Quaternion.identity);
            item.transform.SetParent(slots[i].transform);
        }

        DraggableItem[] hotSlots = transform.Find("HotbarBox").GetComponentsInChildren<DraggableItem>();
        for (int i = 0; i < Mathf.Min(Inventory.hotbars.Count, hotSlots.Length); i++)
        {
            DraggableItem item = Instantiate(draggableItem, transform.position, Quaternion.identity);
            item.transform.SetParent(hotSlots[i].transform);
        }

        DraggableItem handledSlot = transform.Find("HandledBox").GetComponentInChildren<DraggableItem>();
        if (Inventory.holdedItem != null)
        {
            DraggableItem item = Instantiate(draggableItem, transform.position, Quaternion.identity);
            item.transform.SetParent(handledSlot.transform);
        }
    }

}
