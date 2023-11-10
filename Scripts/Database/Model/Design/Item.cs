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
            Sprite = itemScriptable.Sprite;
            Rarity = itemScriptable.Rarity;
        }
/*
        public Item(IDataReader reader) : base(reader)
        {
            SpritePath = reader["sprite_path"].ToString();
            SpriteScale = float.Parse(reader["sprite_scale"].ToString());
            Weight = float.Parse(reader["weight"].ToString());
            Description = reader["description"].ToString();
            TypeName = reader["item_type"].ToString();
            RarityName = reader["rarity_code"].ToString();

            PostInstanciation();
        }        */

        public static Item InstanciateFromJsonString(string json)
        {
            Item item = JsonUtility.FromJson<Item>(json);

            //item.PostInstanciation();

            return item;
        }

        /*
        public void PostInstanciation()
        {
            ItemType = (ItemTypeEnum)Enum.Parse(typeof(ItemTypeEnum), TypeName);
            Sprite = (SpritePath == null) ? null : Resources.Load<Sprite>($"Art/{SpritePath}");
            Rarity = (RarityEnum)Enum.Parse(typeof(RarityEnum), RarityName);
        }
        */
    }
}
