using Assets.Scripts.Enums;
using System;
using System.Data;
using UnityEngine;

namespace Assets.Database.Model.Design
{
    [Serializable]
    public class Weapon : Item
    {
        public string WeaponTypeName;
        public WeaponTypeEnum weaponType;
        public float attackDelay = 1;
        public float speed = 1; //1 is the animator normal speed

        public Weapon(IDataReader reader) : base(reader)
        {
            WeaponTypeName = reader["weapon_type"].ToString();
            attackDelay = float.Parse(reader["attack_delay"].ToString());
            speed = float.Parse(reader["speed"].ToString());

            PostInstanciation();
        }

        public static new Weapon InstanciateFromJsonString(string json)
        {
            Weapon weapon = JsonUtility.FromJson<Weapon>(json);

            weapon.PostInstanciation();

            return weapon;
        }

        protected new void PostInstanciation()
        {
            base.PostInstanciation();
            weaponType = (WeaponTypeEnum)Enum.Parse(typeof(WeaponTypeEnum), WeaponTypeName);
        }

    }
}
