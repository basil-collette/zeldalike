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

        public Weapon(WeaponScriptable weaponScriptable) : base(weaponScriptable)
        {
            weaponType = weaponScriptable.WeaponType;
            attackDelay = weaponScriptable.AttackDelay;
            speed = weaponScriptable.Speed;
        }

    }
}
