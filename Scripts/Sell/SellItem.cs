using Assets.Database.Model.Design;
using Assets.Scripts.Enums;
using Assets.Scripts.Manager;
using UnityEngine;

public class SellItem : MonoBehaviour
{
    public int price;

    public string itemNameCode;
    public ItemTypeEnum itemType;

    public Item item;

    private void Start()
    {
        item = ItemManager.GetItem(itemNameCode, itemType);
    }

    public void Buy()
    {
        var inventory = MainGameManager._inventoryManager;
        Toast toast = new Toast("", ToastType.Success);

        if (inventory._money >= price)
        {
            inventory._money -= price;
            inventory.AddItem(item);
            MainGameManager._storyEventManager.AddShopEvent(item.NameCode);

            toast.Text = $"{item.NameLibelle} acheté !";

            Destroy(gameObject);

            MainGameManager._soundManager.PlayEffect("purchase");
        }
        else
        {
            toast.Text = "Pas assez d'argent!";
        }

        MainGameManager._toastManager.Add(toast);
    }

}
