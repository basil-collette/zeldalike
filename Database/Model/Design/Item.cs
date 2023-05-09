using System;
using System.Data;
using UnityEngine;

namespace Assets.Database.Model.Design
{
    [Serializable]
    public class Item : BaseDbData
    {
        public string SpriteName;
        public float Weight;
        public string Description;

        public Sprite Sprite;
        public RarityEnum Rarity;

        public Item(IDataReader reader) : base(reader)
        {
            SpriteName = reader["sprite_name"].ToString();
            Weight = float.Parse(reader["weight"].ToString());
            Description = reader["description"].ToString();

            Sprite = (SpriteName == null) ? null : Resources.Load<Sprite>(SpriteName);
            Rarity = (RarityEnum)Enum.Parse(typeof(RarityEnum), reader["rarity_code"].ToString());
        }

    }
}
