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

        [HideInInspector] public Vector2 direction;
        [HideInInspector] public Animator anim;
        protected bool attacking;
        protected AliveEntity player;
        protected PlayerInput playerInputs;
        protected CooldownManager cooldownManager;

        protected void Start()
        {
            player = GetComponentInParent<AliveEntity>();
            playerInputs = GetComponent<PlayerInput>();
            cooldownManager = GetComponent<CooldownManager>();
            anim = GetComponentInChildren<Animator>();
            anim.speed = weapon.speed;
            //anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(weapon.animatorName);

            direction = Vector2.zero;
        }

        protected void Update()
        {
            direction = playerInputs.actions["Aim"].ReadValue<Vector2>();
            if (direction != Vector2.zero)
            {
                if (!attacking)
                {
                    attacking = true;
                    player.attacking = true;

                    Attack(direction);

                    Action OnEnd = () => { attacking = false; player.attacking = false; direction = Vector2.zero; };

                    cooldownManager.StartCooldown("attackCooldown", weapon.attackDelay, null, OnEnd);
                }
            }
        }

        protected abstract void Attack(Vector3 direction);

    }
}