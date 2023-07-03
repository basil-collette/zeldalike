using Assets.Scripts.Enums;
using Assets.Scripts.Manager;
using System;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Database.Model.Design
{
    [Serializable]
    public class Weapon : Item
    {
        public Animator anim;
        public WeaponTypeEnum weaponType;
        public float attackDelay = 1;
        public float speed = 1; //1 is the animator normal speed

        bool attacking;
        PlayerInput playerInputs;
        CooldownManager cooldownManager;

        public Weapon(IDataReader reader) : base(reader)
        {
            weaponType = (WeaponTypeEnum)Enum.Parse(typeof(WeaponTypeEnum), reader["weapon_type"].ToString());
            attackDelay = float.Parse(reader["attack_delay"].ToString());
            speed = float.Parse(reader["speed"].ToString());
        }

        public Weapon() : base()
        {
            //Sprite = (SpriteName == null) ? null : Resources.Load<Sprite>($"Art/{SpriteName}");
        }

    }
}