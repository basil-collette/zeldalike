using Assets.Database.Model.Design;
using Assets.Scripts.Enums;
using Assets.Scripts.Manager;
using UnityEngine;

public class SellItem : MonoBehaviour
{
    public int price;

    public string itemNameCode;
    public ItemTypeEnum itemType;

    public InventoryManager inventory;

    Item item;

    private void Start()
    {
        item = ItemManager.GetItem(itemNameCode, itemType);
    }

    public void Buy()
    {
        Toast toast = new Toast("", ToastType.Success);

        if (inventory._money > price)
        {
            inventory._money -= price;
            inventory.AddItem(item);

            toast.Text = $"{item.NameLibelle} acheté !";

            Destroy(gameObject);

            FindGameObjectHelper.FindByName("Main Sound Manager").GetComponent<SoundManager>().PlayEffect("purchase");
        }
        else
        {
            toast.Text = "Pas assez d'argent!";
        }

        FindGameObjectHelper.FindByName("Main Game Manager").GetComponent<ToastManager>().Add(toast);
    }

}
