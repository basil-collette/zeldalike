using Assets.Database.Model.Design;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGuiManager : SingletonGameObject<InventoryGuiManager>
{
    public DraggableItem DraggableItemPrefab;

    public Transform ItemsSlotsContainer;
    public Transform HotbarSlotsContainer;
    public GameObject HoldedSlot;
    public Text MoneyAmount;
    
    InventoryManager Inventory;

    private void OnEnable()
    {
        Inventory = MainGameManager._inventoryManager;

        MoneyAmount.text = Inventory._money.ToString();

        FillSlots();
    }

    private void OnDisable()
    {
        UpdateInventory();
    }

    void FillSlots()
    {
        foreach (Item item in Inventory._items)
        {
            DraggableItem draggableItem = Instantiate(DraggableItemPrefab, transform.position, Quaternion.identity);
            Vector3 itemScale = draggableItem.transform.localScale;
            draggableItem.transform.SetParent(ItemsSlotsContainer.GetChild(item.InventoryIndex).transform);
            draggableItem.transform.localScale = itemScale;
            draggableItem.SetItem(item);
            draggableItem.Image.sprite = draggableItem.Item.Sprite;
            draggableItem.Image.preserveAspect = true;
        }

        foreach (Item item in Inventory._hotbars)
        {
            DraggableItem hotbarItem = Instantiate(DraggableItemPrefab, transform.position, Quaternion.identity);
            Vector3 hotbarScale = hotbarItem.transform.localScale;
            hotbarItem.transform.SetParent(HotbarSlotsContainer.GetChild(item.InventoryIndex).transform);
            hotbarItem.transform.localScale = hotbarScale;
            hotbarItem.SetItem(item);
            hotbarItem.Image.sprite = hotbarItem.Item.Sprite;
            hotbarItem.Image.preserveAspect = true;
        }

        if (Inventory._weapon == null || Inventory._weapon.Id == 0) return;
        DraggableItem weapon = Instantiate(DraggableItemPrefab, transform.position, Quaternion.identity);
        Vector3 weaponScale = weapon.transform.localScale;
        weapon.transform.SetParent(HoldedSlot.transform);
        weapon.transform.localScale = weaponScale;
        weapon.SetItem(Inventory._weapon);
        weapon.Image.sprite = Inventory._weapon.Sprite;
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
