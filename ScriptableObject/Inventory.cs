using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using Assets.Scripts.Enums;
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
    public Weapon Weapon;
    public List<HoldableItem> Hotbars = new List<HoldableItem>();
    public List<Item> Items = new List<Item>();

    public float maxWeight = 10f;
    public float currentWeight = 0;

    void OnValidate()
    {
        //
    }

    public void GetReward(Rewards reward)
    {
        Money += reward.Money;
        Experience += reward.Experience;
        foreach (ItemRef itemRef in reward.ItemsRef)
        {
            switch (itemRef.itemType)
            {
                case ItemTypeEnum.weapon:
                    AddItem(Singleton<WeaponRepository>.Instance.GetByCode(itemRef.ItemCode));
                    break;
                //case ItemTypeEnum.holdable: break;
                default:
                    AddItem(Singleton<ItemRepository<Item>>.Instance.GetByCode(itemRef.ItemCode));
                    break;
            }
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

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }

}
