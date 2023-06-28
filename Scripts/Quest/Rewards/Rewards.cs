using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Rewards
{
    public float Money;
    public float Xp;
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
    public string ItemName;
    public int Amount;
}