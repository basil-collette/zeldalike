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
        for (int i = 0; i < Inventory.Items.Length; i++)
        {
            if (Inventory.Items[i] == null) continue;
            DraggableItem item = Instantiate(DraggableItemPrefab, transform.position, Quaternion.identity);
            Vector3 itemScale = item.transform.localScale;
            item.transform.SetParent(ItemsSlotsContainer.GetChild(i).transform);
            item.transform.localScale = itemScale;
            item.Item = Inventory.Items[i];
            item.image.sprite = item.Item.Sprite;
            item.image.preserveAspect = true;
        }

        for (int i = 0; i < Inventory.Hotbars.Length; i++)
        {
            if (Inventory.Hotbars[i] == null) continue;
            DraggableItem hotbarItem = Instantiate(DraggableItemPrefab, transform.position, Quaternion.identity);
            Vector3 hotbarScale = hotbarItem.transform.localScale;
            hotbarItem.transform.SetParent(HotbarSlotsContainer.GetChild(i).transform);
            hotbarItem.transform.localScale = hotbarScale;
            hotbarItem.Item = Inventory.Hotbars[i];
            hotbarItem.image.sprite = hotbarItem.Item.Sprite;
            hotbarItem.image.preserveAspect = true;
        }

        if (Inventory.Weapon == null) return;
        DraggableItem weapon = Instantiate(DraggableItemPrefab, transform.position, Quaternion.identity);
        Vector3 weaponScale = weapon.transform.localScale;
        weapon.transform.SetParent(HoldedSlot.transform);
        weapon.transform.localScale = weaponScale;
        weapon.Item = Inventory.Weapon;
        weapon.image.sprite = Inventory.Weapon.Sprite;
        weapon.image.preserveAspect = true;
    }

    void UpdateInventory()
    {
        for (int i = 0; i < ItemsSlotsContainer.childCount; i++)
        {
            if (ItemsSlotsContainer.GetChild(i).childCount == 0)
            {
                Inventory.Items[i] = null;
                continue;
            }

            Inventory.Items[i] = ItemsSlotsContainer.GetChild(i).GetChild(0).GetComponent<DraggableItem>().Item;
        }

        for (int i = 0; i < HotbarSlotsContainer.childCount; i++)
        {
            if (HotbarSlotsContainer.GetChild(i).childCount == 0)
            {
                Inventory.Items[i] = null;
                continue;
            }

            Inventory.Hotbars[i] = HotbarSlotsContainer.GetChild(i).GetChild(0).GetComponent<DraggableItem>().Item as HoldableItem;
        }

        var holded = HoldedSlot.GetComponent<DraggableItem>();
        Inventory.Weapon = (holded == null) ? null : holded.Item as Weapon;
    }    

}
