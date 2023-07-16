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

        public string SpriteName;
        public Sprite Sprite;

        public string RarityName;
        public RarityEnum Rarity;

        public Item(IDataReader reader) : base(reader)
        {
            SpriteName = reader["sprite_name"].ToString();
            Weight = float.Parse(reader["weight"].ToString());
            Description = reader["description"].ToString();
            TypeName = reader["item_type"].ToString();
            RarityName = reader["rarity_code"].ToString();

            PostInstanciation();
        }        

        public static Item InstanciateFromJsonString(string json)
        {
            Item item = JsonUtility.FromJson<Item>(json);

            item.PostInstanciation();

            return item;
        }

        protected void PostInstanciation()
        {
            ItemType = (ItemTypeEnum)Enum.Parse(typeof(ItemTypeEnum), TypeName);
            Sprite = (SpriteName == null) ? null : Resources.Load<Sprite>($"Art/{SpriteName}");
            Rarity = (RarityEnum)Enum.Parse(typeof(RarityEnum), RarityName);
        }

    }
}
