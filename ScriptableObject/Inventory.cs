#nullable enable

using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Inventory")]
public class Inventory : ScriptableObject
{
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
    public static Inventory GetInventory() { return FindAnyObjectByType<Player>().inventory; }

    public static Item GetItem(string itemParams) // dataTableName;itemName
    {
        string[] itemParamsArray = itemParams.Split(";");

        ItemTypeEnum type = (ItemTypeEnum)Enum.Parse(typeof(ItemTypeEnum), itemParamsArray[0]);
        string name = itemParamsArray[1];

        return GetItem(name, type);
    }

    public static Item GetItem(string itemName, ItemTypeEnum type)
    {
        switch (type)
        {
            case ItemTypeEnum.weapon:
                return Singleton<WeaponRepository>.Instance.GetByCode(itemName);

            case ItemTypeEnum.item:
                return Singleton<ItemRepository<Item>>.Instance.GetByCode(itemName);

            default: return null;
        }
    }

    public static void AddItem(string itemParams) { GetInventory().AddItem(GetItem(itemParams)); }

    public static void StaticRemoveItem(string itemname) { GetInventory().RemoveItem(itemname); }
    #endregion 

    public void GetReward(Rewards reward)
    {
        if (reward.Money != null) Money += reward.Money;

        if (reward.Experience != null) Experience += reward.Experience;

        if (reward.ItemsRef == null) return;
        foreach (ItemRef itemRef in reward.ItemsRef)
        {
            AddItem(GetItem(itemRef.ItemCode, itemRef.itemType));
        }
    }

    public void AddItem(Item content)
    {
        if (Items.Count < maxCountItems)
        {
            Items.Add(content);
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
