using Assets.Database.Model.Repository;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Enums;

[System.Serializable]
public class Rewards
{
    public int Money;
    public int Experience;
    public List<ItemRef> ItemsRef;
    //[HideInInspector] public List<Item> Items;
    //public List<string> Spells;

    /*
    public void OnAfterDeserialize()
    {
        Items = new List<Item>();
        foreach (var itemRef in ItemsRef)
        {
            Item item = ItemRepository.Current.GetByCode(itemRef.ItemName);
            Items.Add(item);
        }
    }
    */

}

[System.Serializable]
public class ItemRef
{
    public string ItemCode;
    public int Amount;
    public ItemTypeEnum itemType;
}