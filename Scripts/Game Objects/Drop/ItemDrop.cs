using Assets.Database.Model.Design;
using UnityEngine;

public class ItemDrop : Drop
{
    public Item item;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.Sprite;
    }

    protected override void OnTriggerEnter2DIsPlayer(Collider2D collider)
    {
        collider.GetComponent<Player>().inventory.AddItem(item);
    }

}
