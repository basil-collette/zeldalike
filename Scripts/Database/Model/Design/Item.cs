using Assets.Scripts.Enums;
using System;
using System.Data;
using UnityEngine;

namespace Assets.Database.Model.Design
{
    [Serializable]
    public class Item : BaseDbData
    {
        public int InventoryIndex;
        
        public float Weight;
        public string Description;

        public string TypeName;
        public ItemTypeEnum ItemType;

        public string SpritePath;
        public float SpriteScale;
        public Sprite Sprite;

        public string RarityName;
        public RarityEnum Rarity;

        public Item(ItemScriptable itemScriptable) : base (itemScriptable)
        {
            Weight = itemScriptable.Weight;
            Description = itemScriptable.Description;
            ItemType = itemScriptable.ItemType;
            SpriteScale = itemScriptable.SpriteScale;
            SpritePath = itemScriptable.SpritePath;
            Rarity = itemScriptable.Rarity;

            PostInstanciation();
        }

        public void PostInstanciation()
        {
            Sprite = Resources.Load<Sprite>($"Art/{SpritePath}");
        }

    }

}
