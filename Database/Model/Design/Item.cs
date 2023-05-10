using Assets.Scripts.Enums;
using System;
using System.Data;
using UnityEngine;

namespace Assets.Database.Model.Design
{
    [Serializable]
    public class Item : BaseDbData
    {
        public string NameLibelle;
        public string NameCode;
        public string SpriteName;
        public float Weight;
        public string Description;

        public ItemTypeEnum ItemType;
        public Sprite Sprite;
        public RarityEnum Rarity;

        public Item(IDataReader reader) : base(reader)
        {
            NameLibelle = reader["name_libelle"].ToString();
            NameCode = reader["name_code"].ToString();
            SpriteName = reader["sprite_name"].ToString();
            Weight = float.Parse(reader["weight"].ToString());
            Description = reader["description"].ToString();

            ItemType = (ItemTypeEnum)Enum.Parse(typeof(ItemTypeEnum), reader["item_type"].ToString());
            Sprite = (SpriteName == null) ? null : Resources.Load<Sprite>($"Art/{SpriteName}");
            Rarity = (RarityEnum)Enum.Parse(typeof(RarityEnum), reader["rarity_code"].ToString());
        }

    }
}
