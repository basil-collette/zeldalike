using Assets.Database.Model.Design;
using Assets.Scripts.Manager;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Items.Equipments.Weapons
{
    [Serializable]
    public abstract class WeaponMonoBehaviour : MonoBehaviour
    {
        public Weapon weapon;

        [HideInInspector] public Animator anim;
        protected bool attacking;
        protected PlayerInput playerInputs;
        protected CooldownManager cooldownManager;

        protected void Start()
        {
            playerInputs = GetComponent<PlayerInput>();
            cooldownManager = GetComponent<CooldownManager>();
            anim = GetComponentInChildren<Animator>();
            anim.speed = weapon.speed;
            //anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(weapon.animatorName);
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

                    cooldownManager.StartCooldown("attackCooldown", weapon.attackDelay, null, OnEnd);
                }
            }
        }

        protected abstract void Attack(Vector3 direction);

    }
}