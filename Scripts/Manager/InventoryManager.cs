using Assets.Database.Model.Design;
using UnityEngine;

public class InventoryManager : SignletonGameObject<InventoryManager>
{
    public DraggableItem DraggableItemPrefab;

    public Transform ItemsSlotsContainer;
    public Transform HotbarSlotsContainer;
    public GameObject HoldedSlot;
    
    Inventory Inventory;

    private void OnEnable()
    {
        Inventory = FindGameObjectHelper.FindByName("Player").GetComponent<Player>().inventory;

        FillSlots();
    }

    private void OnDisable()
    {
        UpdateInventory();
    }

    void FillSlots()
    {
        foreach (Item item in Inventory.Items)
        {
            DraggableItem draggableItem = Instantiate(DraggableItemPrefab, transform.position, Quaternion.identity);
            Vector3 itemScale = draggableItem.transform.localScale;
            draggableItem.transform.SetParent(ItemsSlotsContainer.GetChild(item.InventoryIndex).transform);
            draggableItem.transform.localScale = itemScale;
            draggableItem.SetItem(item);
            draggableItem.Image.sprite = draggableItem.Item.Sprite;
            draggableItem.Image.preserveAspect = true;
        }

        foreach (Item item in Inventory.Hotbars)
        {
            DraggableItem hotbarItem = Instantiate(DraggableItemPrefab, transform.position, Quaternion.identity);
            Vector3 hotbarScale = hotbarItem.transform.localScale;
            hotbarItem.transform.SetParent(HotbarSlotsContainer.GetChild(item.InventoryIndex).transform);
            hotbarItem.transform.localScale = hotbarScale;
            hotbarItem.SetItem(item);
            hotbarItem.Image.sprite = hotbarItem.Item.Sprite;
            hotbarItem.Image.preserveAspect = true;
        }

        if (Inventory.Weapon == null) return;
        DraggableItem weapon = Instantiate(DraggableItemPrefab, transform.position, Quaternion.identity);
        Vector3 weaponScale = weapon.transform.localScale;
        weapon.transform.SetParent(HoldedSlot.transform);
        weapon.transform.localScale = weaponScale;
        weapon.SetItem(Inventory.Weapon);
        weapon.Image.sprite = Inventory.Weapon.Sprite;
        weapon.Image.preserveAspect = true;
    }

    void UpdateInventory()
    {
        /*
        for (int i = 0; i < ItemsSlotsContainer.childCount; i++)
        {
            if (ItemsSlotsContainer.GetChild(i).childCount == 0)
            {
                Inventory.Items[i] = null;
                continue;
            }

            Inventory.Items[i] = ItemsSlotsContainer.GetChild(i).GetComponentInChildren<DraggableItem>().Item;
        }

        for (int i = 0; i < HotbarSlotsContainer.childCount; i++)
        {
            if (HotbarSlotsContainer.GetChild(i).childCount == 0)
            {
                Inventory.Items[i] = null;
                continue;
            }

            Inventory.Hotbars[i] = HotbarSlotsContainer.GetChild(i).GetComponentInChildren<DraggableItem>().Item as HoldableItem;
        }

        var holded = HoldedSlot.GetComponentInChildren<DraggableItem>();
        Inventory.Weapon = (holded == null) ? null : holded.Item as Weapon;
        */
    }    

}
