#nullable enable

using Assets.Database.Model.Design;
using Assets.Scripts.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Inventory")]
public class Inventory : ScriptableObject
{
    public static event Action<string[]> OnObtain;

    public int Money = 0;
    public int Experience = 0;
    //OBJETS
    public static int MaxCountHotbar = 3;
    public static int maxCountItems = 12;
    public Weapon? Weapon;
    public List<HoldableItem> Hotbars = new List<HoldableItem>();
    public List<Item> Items = new List<Item>();

    public float maxWeight = 10f;
    public float currentWeight = 0;

    #region EasyAccess
    public static Inventory GetInventory() => FindAnyObjectByType<Player>().inventory;

    public static void AddItem(string itemParams) {
        var item = ItemManager.GetItem(itemParams);
        GetInventory().AddItem(item);

        OnObtain?.Invoke(new string[] { item.NameCode });
    }

    public static void StaticRemoveItem(string itemname) { GetInventory().RemoveItem(itemname); }
    #endregion 

    public void GetReward(Rewards reward)
    {
        if (reward.Money != null) Money += reward.Money;

        if (reward.Experience != null) Experience += reward.Experience;

        if (reward.ItemsRef == null) return;
        foreach (ItemRef itemRef in reward.ItemsRef)
        {
            AddItem(ItemManager.GetItem(itemRef.ItemCode, itemRef.itemType));
        }
    }

    public void AddItem(Item content)
    {
        if (Items.Count < maxCountItems)
        {
            Items.Add(content);
            OnObtain?.Invoke(new string[] { content.NameCode });
        }
        else
        {
            // ask for bin an item

            //pause and open inventory scene
        }
    }

    public void RemoveItem(string itemName)
    {
        Items.Remove(Items.Find(x => x.NameLibelle == itemName));
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }

}
