using Assets.Scripts.Enums;
using System.Collections.Generic;

[System.Serializable]
public class Rewards
{
    public float Money;
    public float Experience;
    public List<ItemRef> ItemsRef;
}

[System.Serializable]
public class ItemRef
{
    public string ItemCode;
    public int Amount;
    public ItemTypeEnum itemType; //to know the db table where to search
}