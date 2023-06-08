using Assets.Database.Model.Design;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObject/Inventory")]
public class Inventory : ScriptableObject
{
    public HoldableItem holdedItem; //public HoldableItem hand;
    public List<Item> items = new List<Item>();
    public float maxWeight = 10f;
    public float currentWeight = 0;

    //public List<HoldableItem> hotbar = new List<HoldableItem>();

    void OnValidate()
    {
        var attackJoystickImage = GameObject.Find("HoldItemSprite")?.GetComponent<Image>();
        
        if (attackJoystickImage != null)
            attackJoystickImage.sprite = holdedItem.Sprite;
    }

}
