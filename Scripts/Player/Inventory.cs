using Assets.Database.Model.Design;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Inventory")]
public class Inventory : ScriptableObject
{
    public Item holdedItem;
    public List<Item> items = new List<Item>();
    public float maxWeight = 10f;
    public float currentWeight = 0;

    

}
