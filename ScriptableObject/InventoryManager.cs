#nullable enable

using Assets.Database.Model.Design;
using Assets.Scripts.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<StoryEventManager>, ISavable
{
    public static event Action<string[]> OnObtain;

    public int _money = 0;
    public int _experience = 0;

    //Weight
    public float _maxWeight = 10f;
    public float _currentWeight = 0;

    //OBJETS
    public static int MaxCountHotbar = 3;
    public static int maxCountItems = 12;

    public Weapon? _weapon;
    public List<HoldableItem> _hotbars = new List<HoldableItem>();
    public List<Item> _items = new List<Item>();

    public void GetReward(Rewards reward)
    {
        if (reward.Money != null) _money += reward.Money;

        if (reward.Experience != null) _experience += reward.Experience;

        if (reward.ItemsRef == null) return;
        foreach (ItemRef itemRef in reward.ItemsRef)
        {
            AddItem(ItemManager.GetItem(itemRef.ItemCode, itemRef.itemType));
        }
    }

    public void AddItem(string itemParams)
    {
        var item = ItemManager.GetItem(itemParams);
        AddItem(item);
    }

    public void AddItem(Item content)
    {
        if (_items.Count < maxCountItems)
        {
            content.InventoryIndex = GetFirstFreeIndexSlot();
            _items.Add(content);
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
        _items.Remove(_items.Find(x => x.NameLibelle == itemName));
    }

    public void AddMoney(int amount)
    {
        _money += amount;
    }

    public void RemoveMoney(int amount)
    {
        if (_money > amount) _money -= amount;
    }

    public void RemoveItem(Item item)
    {
        _items.Remove(item);
    }

    int GetFirstFreeIndexSlot()
    {
        for (int i = 0; i <= _items.Count; i++)
        {
            if (!_items.Exists(x => x.InventoryIndex == i))
            {
                return i;
            }
        }
        return 0;
    }

    public string ToJsonString()
    {
        return JsonUtility.ToJson(Get());
    }

    public void Load(string json)
    {
        Set(JsonUtility.FromJson<InventorySaveModel>(json));
    }

    public InventorySaveModel Get()
    {
        return new InventorySaveModel
        {
            Money = _money,
            Experience = _experience,
            MaxWeight = _maxWeight,
            CurrentWeight = _currentWeight,
            Weapon = _weapon,
            Hotbars = _hotbars,
            Items = _items,
        };
    }

    public void Set(InventorySaveModel saveModel)
    {
        _money = saveModel.Money;
        _experience = saveModel.Experience;
        _maxWeight = saveModel.MaxWeight;
        _currentWeight = saveModel.CurrentWeight;

        _weapon = saveModel.Weapon;
        _weapon?.PostInstanciation();

        _hotbars = saveModel.Hotbars;
        _hotbars.ForEach(x => x.PostInstanciation());

        _items = saveModel.Items;
        _items.ForEach(x => x.PostInstanciation());
    }

}

[Serializable]
public class InventorySaveModel
{
    public int Money;
    public int Experience;
    public float MaxWeight = 10f;
    public float CurrentWeight;
    public Weapon? Weapon;
    public List<HoldableItem> Hotbars = new List<HoldableItem>();
    public List<Item> Items = new List<Item>();
}