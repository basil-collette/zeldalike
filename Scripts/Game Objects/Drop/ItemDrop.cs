using Assets.Database.Model.Design;
using Assets.Scripts.Enums;
using Assets.Scripts.Manager;
using UnityEngine;

public class ItemDrop : Drop
{
    [HideInInspector] [SerializeReference] public Item Item;
    public string itemNameCode;
    public ItemTypeEnum itemType;

    private void Start()
    {
        Item = ItemManager.GetItem(itemNameCode, itemType);
        GetComponent<SpriteRenderer>().sprite = Item.Sprite;
    }

    protected override void OnTriggerEnter2DIsPlayer(Collider2D collider)
    {
        MainGameManager._inventoryManager.AddItem(Item);
    }

}
