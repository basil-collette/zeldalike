using Assets.Database.Model.Design;
using Assets.Scripts.Enums;
using Assets.Scripts.Manager;
using UnityEngine;

public class SellItem : MonoBehaviour
{
    public int price;

    public string itemNameCode;
    public ItemTypeEnum itemType;

    public Inventory inventory;

    Item item;

    private void Start()
    {
        item = ItemManager.GetItem(itemNameCode, itemType);
    }

    public void Buy()
    {
        Toast toast = new Toast("", ToastType.Success);

        if (inventory.Money > price)
        {
            inventory.Money -= price;
            inventory.AddItem(item);

            toast.Text = $"{item.NameLibelle} acheté !";

            Destroy(gameObject);
        }
        else
        {
            toast.Text = "Pas assez d'argent!";
        }

        FindGameObjectHelper.FindByName("Main Game Manager").GetComponent<ToastManager>().Add(toast);
    }

}
