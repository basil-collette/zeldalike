using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using Assets.Scripts.Enums;
using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObject/Inventory")]
public class Inventory : ScriptableObject
{
    public int Money = 0;
    public int Experience = 0;
    //OBJETS
    public Weapon? Weapon;
    public HoldableItem[] Hotbars = new HoldableItem[3];
    public Item?[] Items = new Item?[12];

    public float maxWeight = 10f;
    public float currentWeight = 0;

    void OnValidate()
    {
        //REPLACE BY GETTING FROM SAVE

        for (int i = 0; i < Items.Length; i++)
        {
            Items[i] = null;
        }
        for (int i = 0; i < Hotbars.Length; i++)
        {
            Hotbars[i] = null;
        }
        Weapon = Singleton<WeaponRepository>.Instance.GetByCode("sword");
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
        //ne gere pas les stackable objects
        var index = Array.FindIndex(Items, i => i == null);
        if (index == -1)
        {
            //ask for bin an item
            return;
        }
        Items[index] = content;
    }

}
