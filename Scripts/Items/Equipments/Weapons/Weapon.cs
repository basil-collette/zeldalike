using Assets.Scripts.Manager;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Items.Equipments.Weapons
{
    [Serializable]
    public abstract class Weapon : HoldableItem
    {
        public Animator anim;
        public float attackDelay = 1;
        public float speed = 1; //1 is the animator normal speed

        bool attacking;
        PlayerInput playerInputs;
        CooldownManager cooldownManager;

        protected void Start()
        {
            playerInputs = GetComponent<PlayerInput>();
            cooldownManager = GetComponent<CooldownManager>();
            anim.speed = speed;
        }

        protected void Update()
        {
            Vector2 attackJoystickInput = playerInputs.actions["Aim"].ReadValue<Vector2>();
            if (attackJoystickInput != Vector2.zero)
            {
                if (!attacking)
                {
                    attacking = true;

                    Attack(attackJoystickInput);

                    Action OnEnd = () => { attacking = false; };

                    cooldownManager.StartCooldown("attackCooldown", attackDelay, null, OnEnd);
                }
            }
        }

        protected abstract void Attack(Vector3 direction);

    }
}