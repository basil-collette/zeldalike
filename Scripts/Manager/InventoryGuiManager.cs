using Assets.Database.Model.Design;
using Assets.Scripts.Enums;
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

    void FillSlots()
    {
        foreach (Item item in Inventory._items)
        {
            Transform slot = ItemsSlotsContainer.GetChild(item.InventoryIndex).transform;

            DraggableItem draggableItem = Instantiate(DraggableItemPrefab, slot);
            draggableItem.SetItem(item);
            draggableItem.Image.sprite = draggableItem.Item.Sprite;
            draggableItem.Image.preserveAspect = true;

            if (item.ItemType == ItemTypeEnum.weapon)
            {
                draggableItem.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 135);
            }
        }

        /*
        foreach (Item item in Inventory._hotbars)
        {
            Transform slot = ItemsSlotsContainer.GetChild(item.InventoryIndex).transform;

            DraggableItem hotbarItem = Instantiate(DraggableItemPrefab, slot);
            hotbarItem.SetItem(item);
            hotbarItem.Image.sprite = hotbarItem.Item.Sprite;
            hotbarItem.Image.preserveAspect = true;
        }
        */

        if (Inventory._weapon == null || Inventory._weapon.Id == 0) return;

        DraggableItem weapon = Instantiate(DraggableItemPrefab, HoldedSlot.transform);
        weapon.SetItem(Inventory._weapon);
        weapon.Image.sprite = Inventory._weapon.Sprite;
        weapon.Image.preserveAspect = true;

        weapon.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 135);
    }   

}
