using Assets.Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Items/Item")]
public class ItemScriptable : DbDataScriptable
{
    public float Weight;
    public string Description;

    public ItemTypeEnum ItemType;

    public float SpriteScale;
    public Sprite Sprite;

    public RarityEnum Rarity;
}
